using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serenno.Domain;
using Serenno.Domain.Models.Core;
using Serenno.Services.Accounts;
using Serenno.Services.Users;
using Serilog;

namespace Serenno.Services.Energy
{
    public sealed record GetAllEnergyLevelsRequest(ulong DiscordUserId, uint? Allycode = null) : IRequest<ServiceResponse<List<UserEnergy>>>;

    public sealed record SetEnergyLevelRequest(ulong DiscordUserId, EnergyDto Dto) : IRequest<ServiceResponse>;

    public sealed record SetBulkEnergyLevelRequest(ulong DiscordUserId, List<EnergyDto> Dtos) : IRequest<ServiceResponse>;

    public sealed record InvalidateUserEnergyLevels(ulong DiscordUserId, uint Allycode) : IRequest;
    
    public class EnergyHandler : 
        RequestHandler<InvalidateUserEnergyLevels>,
        IRequestHandler<GetAllEnergyLevelsRequest, ServiceResponse<List<UserEnergy>>>,
        IRequestHandler<SetEnergyLevelRequest, ServiceResponse>,
        IRequestHandler<SetBulkEnergyLevelRequest, ServiceResponse>
    {
        private readonly SerennoContext _context;
        private readonly IMediator _mediator;
        private readonly IAppCache _appCache;
        private readonly ILogger _logger;

        public EnergyHandler(SerennoContext context, IMediator mediator, IAppCache appCache, ILogger logger)
        {
            _context = context;
            _mediator = mediator;
            _appCache = appCache;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<UserEnergy>>> Handle(GetAllEnergyLevelsRequest request, CancellationToken cancellationToken)
        {
            var userExists = await _mediator.Send(new EnsureUserExistsRequest(request.DiscordUserId), cancellationToken);
            var userAccount = await _mediator.Send(new GetPrimaryAccountRequest(request.DiscordUserId), cancellationToken);

            if (userAccount.Failure)
                return ServiceResponse.Fail<List<UserEnergy>>("User does not exist; and does not have energy levels");
            
            var userEnergies = await _appCache.GetOrAddAsync(BuildEnergyLevelCacheKey(request.DiscordUserId, userAccount.Value.Allycode),
                () => ReadUserEnergyLevels(request.DiscordUserId, cancellationToken),
                TimeSpan.FromHours(6));

            return ServiceResponse.Ok(userEnergies);
        }

        public async Task<ServiceResponse> Handle(SetEnergyLevelRequest request, CancellationToken cancellationToken)
        {
            var userExists = await _mediator.Send(new EnsureUserExistsRequest(request.DiscordUserId), cancellationToken);
            var userAccount = await _mediator.Send(new GetPrimaryAccountRequest(request.DiscordUserId), cancellationToken);
            
            if(userAccount.Failure)
                return ServiceResponse.Fail(userAccount.ErrorMessage);
            
            var energy = new UserEnergy
            {
                Time = DateTimeOffset.Now,
                EnergyAmount = request.Dto.energyAmount.Value,
                EnergyType = request.Dto.EnergyType,
                AccountId = userAccount.Value.Allycode
            };

            _context.Energies.Add(energy);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Send(new InvalidateUserEnergyLevels(request.DiscordUserId, userAccount.Value.Allycode), cancellationToken);
            return ServiceResponse.Ok();
        }
        
        public async Task<ServiceResponse> Handle(SetBulkEnergyLevelRequest request, CancellationToken cancellationToken)
        {
            _logger.Debug("Fetching primary account");
            var userAccount = await _mediator.Send(new GetPrimaryAccountRequest(request.DiscordUserId), cancellationToken);
            
            if(userAccount.Failure)
                return ServiceResponse.Fail(userAccount.ErrorMessage);
            
            var requests = request.Dtos;
            
            foreach (var dto in requests)
            {
                if(dto.energyAmount is null)
                    continue;

                _context.Energies.Add(new UserEnergy
                {
                    Time = DateTimeOffset.Now,
                    EnergyType = dto.EnergyType,
                    EnergyAmount = dto.energyAmount.Value,
                    AccountId = userAccount.Value.Allycode
                });
            }

            await _mediator.Send(new InvalidateUserEnergyLevels(request.DiscordUserId, userAccount.Value.Allycode), cancellationToken);
            return ServiceResponse.Ok();
        }
        
        protected override void Handle(InvalidateUserEnergyLevels request)
        {
            _appCache.Remove(BuildEnergyLevelCacheKey(request.DiscordUserId, request.Allycode));
        }

        private async Task<List<UserEnergy>> ReadUserEnergyLevels(ulong discordId, CancellationToken cancellationToken = default)
        {
            var dt = DateTimeOffset.UtcNow.AddDays(-1);
            
            return await _context.Energies
                .Where(o => o.Time >= dt)
                .Where(o => o.Id == discordId)
                .GroupBy(o => o.EnergyType)
                .Select(x => x.OrderByDescending(y => y.Time).First())
                .ToListAsync(cancellationToken: cancellationToken);
        }

        private string BuildEnergyLevelCacheKey(ulong discordId, uint allycode)
        {
            return $"{nameof(EnergyHandler)}/{discordId}/{allycode}";
        }
    }
}