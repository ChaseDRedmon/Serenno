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
using Serenno.Domain.Models.Core.Ticket;
using Serenno.Services.Accounts;

namespace Serenno.Services.Ticket
{
    public sealed record InvalidateUserTicketLevels(ulong DiscordUserId, uint Allycode) : IRequest { }
    public sealed record GetAllTicketsRequest(ulong DiscordUserId) : IRequest<ServiceResponse<List<UserTickets>>>;
    public sealed record GetDailyTicketsRequest(ulong DiscordUserId, uint? Allycode = null) : IRequest<ServiceResponse<UserTickets>>;

    public sealed record SetDailyTicketRequest
        (ulong DiscordUserId, ushort TicketAmount, uint? Allycode = null) : IRequest<ServiceResponse>;
    
    public class TicketHandler : 
        RequestHandler<InvalidateUserTicketLevels>,
        IRequestHandler<SetDailyTicketRequest, ServiceResponse>,
        IRequestHandler<GetAllTicketsRequest, ServiceResponse<List<UserTickets>>>,
        IRequestHandler<GetDailyTicketsRequest, ServiceResponse<UserTickets>>
    {
        private readonly SerennoContext _context;
        private readonly IMediator _mediator;
        private readonly IAppCache _appCache;

        public TicketHandler(SerennoContext context, IMediator mediator, IAppCache appCache)
        {
            _context = context;
            _mediator = mediator;
            _appCache = appCache;
        }

        public Task<ServiceResponse<List<UserTickets>>> Handle(GetAllTicketsRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<UserTickets>> Handle(GetDailyTicketsRequest request, CancellationToken cancellationToken)
        {
            AccountDto? account = null;
            
            if (request.Allycode is null)
            {
                var serviceResponse = await _mediator.Send(new GetPrimaryAccountRequest(request.DiscordUserId), cancellationToken);
                if (serviceResponse.Failure)
                    return ServiceResponse.Fail<UserTickets>(serviceResponse.ErrorMessage);

                account = serviceResponse.Value;
            }
            else
            {
                var serviceResponse = await _mediator.Send(new GetAccountRequest(request.Allycode.Value), cancellationToken);
                if (serviceResponse.Failure)
                    return ServiceResponse.Fail<UserTickets>(serviceResponse.ErrorMessage);

                account = serviceResponse.Value;
            }
            
            var userTickets = await _appCache.GetOrAddAsync(BuildDailyTicketCacheKey(request.DiscordUserId, account!.Allycode),
                () => ReadAccountTicketLevel(account!.Allycode),
                TimeSpan.FromHours(6));

            return ServiceResponse.Ok(userTickets);
        }
        
        public async Task<ServiceResponse> Handle(SetDailyTicketRequest request, CancellationToken cancellationToken)
        {
            // If they do not specify an allycode, use the primary account we have on record
            if (request.Allycode is null)
            {
                // Find the primary account here
                var account = await _mediator.Send(new GetPrimaryAccountRequest(request.DiscordUserId), cancellationToken);
                if (account.Failure)
                    return ServiceResponse.Fail(account.ErrorMessage);

                var primaryAccount = account.Value;
                await SetAccountTicketLevel(request.DiscordUserId, primaryAccount!.Allycode, request.TicketAmount);
            }
            else
            {
                var account = await _mediator.Send(new GetAccountRequest(request.Allycode.Value), cancellationToken);
                if (account.Failure)
                    return ServiceResponse.Fail(account.ErrorMessage);

                await SetAccountTicketLevel(request.DiscordUserId, request.Allycode.Value, request.TicketAmount);
            }
            
            return ServiceResponse.Ok();
        }

        private async Task<ServiceResponse> SetAccountTicketLevel(ulong discordUserId, uint allycode, ushort ticketAmount)
        {
            var ticket = new UserTickets
            {
                Date = DateTimeOffset.Now,
                TicketAmount = ticketAmount,
                AccountFK = allycode
            };

            _context.UserTickets.Add(ticket);
            await _context.SaveChangesAsync();

            await _mediator.Send(new InvalidateUserTicketLevels(discordUserId, allycode));
            return ServiceResponse.Ok();
        }
        
        protected override void Handle(InvalidateUserTicketLevels request)
        {
            _appCache.Remove(BuildDailyTicketCacheKey(request.DiscordUserId, request.Allycode));
        }

        private async Task<List<UserTickets>> ReadAllAccountTicketLevel(ulong discordUserId)
        {
            var ticketsWithinPreviousDay = DateTimeOffset.Now.AddDays(-1);

            return await _context.Accounts
                .Where(x => x.UserId == discordUserId)
                .SelectMany(x => x.Tickets)
                .Where(x => x.Date <= ticketsWithinPreviousDay)
                .ToListAsync();
        }

        private async Task<UserTickets> ReadAccountTicketLevel(uint allycode)
        {
            return await _context.UserTickets
                .Where(x => x.AccountFK == allycode)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync();
        }

        private string BuildDailyTicketCacheKey(ulong discordId, uint allycode)
        {
            return $"{nameof(TicketHandler)}/{discordId}/{allycode}";
        }
    }
}