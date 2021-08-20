using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serenno.Domain;
using Serenno.Domain.Models.Core.Guilds;
using Serenno.Services.Users;

namespace Serenno.Services.Accounts
{
    public sealed record GetAllRegisteredAccountsRequest(ulong DiscordUserId) : IRequest<ServiceResponse<List<AccountDto?>>>;
    public sealed record GetAccountRequest(uint Allycode) : IRequest<ServiceResponse<AccountDto?>>;
    public sealed record GetPrimaryAccountRequest(ulong DiscordUserId) : IRequest<ServiceResponse<AccountDto?>>;
    public sealed record RegisterNewAccountRequest(ulong DiscordUserId, uint Allycode, bool IsPrimaryAccount) : IRequest<ServiceResponse>;
    public sealed record RegisterAlternateAccountRequest(ulong DiscordUserId, uint Allycode, bool IsPrimaryAccount) : IRequest<ServiceResponse>;
    public sealed record UpdatePrimaryAccountRequest(ulong DiscordUserId, uint AllyCode) : IRequest<ServiceResponse>;
    public sealed record AccountDto(ulong DiscordId, uint Allycode, string? AccountName, byte? AccountLevel, bool IsPrimaryAccount);
    
    public class AccountHandler : 
        IRequestHandler<RegisterNewAccountRequest, ServiceResponse>,
        IRequestHandler<RegisterAlternateAccountRequest, ServiceResponse>,
        IRequestHandler<UpdatePrimaryAccountRequest, ServiceResponse>,
        IRequestHandler<GetAllRegisteredAccountsRequest, ServiceResponse<List<AccountDto?>>>,
        IRequestHandler<GetAccountRequest, ServiceResponse<AccountDto?>>,
        IRequestHandler<GetPrimaryAccountRequest, ServiceResponse<AccountDto?>>
    {
        private readonly SerennoContext _serennoContext;
        private readonly IMediator _mediator;
        private readonly IAppCache _appCache;

        public AccountHandler(SerennoContext serennoContext, IMediator mediator, IAppCache appCache)
        {
            _serennoContext = serennoContext;
            _mediator = mediator;
            _appCache = appCache;
        }
        
        public async Task<ServiceResponse> Handle(RegisterNewAccountRequest request, CancellationToken cancellationToken)
        {
            var ensureUserExists = await _mediator.Send(new EnsureUserExistsRequest(request.DiscordUserId), cancellationToken);

            var accounts = await _serennoContext
                .Accounts
                .Where(x => x.UserId == request.DiscordUserId)
                .ToListAsync(cancellationToken: cancellationToken);

            foreach (var account in accounts)
            {
                account.IsPrimaryAccount = false;
            }
            
            var newAccount = new Account
            {
                UserId = request.DiscordUserId,
                Allycode = request.Allycode,
                IsPrimaryAccount = request.IsPrimaryAccount
            };

            _serennoContext.Accounts.Add(newAccount);
            await _serennoContext.SaveChangesAsync(cancellationToken);
            await _mediator.Send(new InvalidateAllycodeExistsRequest(request.Allycode), cancellationToken);
            
            return ServiceResponse.Ok();
        }
        
        public async Task<ServiceResponse> Handle(RegisterAlternateAccountRequest request, CancellationToken cancellationToken)
        {
            var ensureUserExists = await _mediator.Send(new EnsureUserExistsRequest(request.DiscordUserId), cancellationToken);
            
            var newAccount = new Account
            {
                UserId = request.DiscordUserId,
                Allycode = request.Allycode,
                IsPrimaryAccount = request.IsPrimaryAccount
            };

            _serennoContext.Accounts.Add(newAccount);
            await _serennoContext.SaveChangesAsync(cancellationToken);
            await _mediator.Send(new InvalidateAllycodeExistsRequest(request.Allycode), cancellationToken);
            
            return ServiceResponse.Ok();
        }
        
        public async Task<ServiceResponse> Handle(UpdatePrimaryAccountRequest request, CancellationToken cancellationToken)
        {
            var userExists = await _mediator.Send(new UserExistsRequest(request.DiscordUserId), cancellationToken);

            if (!userExists)
                return ServiceResponse.Fail("User does not exist");

            var accountExists = await _mediator.Send(new GetAccountRequest(request.AllyCode), cancellationToken);
            if (accountExists.Failure)
                return ServiceResponse.Fail<AccountDto>("SWGOH Account does not exist");

            var accounts = await _serennoContext
                .Accounts
                .Where(x => x.UserId == request.DiscordUserId)
                .ToListAsync(cancellationToken: cancellationToken);

            foreach (var account in accounts)
            {
                account.IsPrimaryAccount = false;
            }

            var newPrimaryAccount = accounts.Single(x => x.Allycode == request.AllyCode);
            newPrimaryAccount.IsPrimaryAccount = true;

            await _serennoContext.SaveChangesAsync(cancellationToken);
            return ServiceResponse.Ok();
        }
        
        public async Task<ServiceResponse<List<AccountDto?>>> Handle(GetAllRegisteredAccountsRequest request, CancellationToken cancellationToken)
        {
            var accountExists = await _mediator.Send(new UserExistsRequest(request.DiscordUserId), cancellationToken);

            if (!accountExists)
                ServiceResponse.Fail("User does not exist");

            var accounts = await _appCache.GetOrAddAsync(BuildGetAllAccountsCacheKey(request.DiscordUserId), 
                () => GetAllAccounts(request.DiscordUserId), 
                TimeSpan.FromDays(30));
                
            if (accounts.Count == 0)
                return ServiceResponse.Fail<List<AccountDto?>>("User does not have any accounts");
            
            return ServiceResponse.Ok(accounts);
        }

        public async Task<ServiceResponse<AccountDto?>> Handle(GetAccountRequest request, CancellationToken cancellationToken)
        {
            var accountExists = await _mediator.Send(new AllycodeExistsRequest(request.Allycode), cancellationToken);

            if (!accountExists)
                ServiceResponse.Fail("Account does not exist");

            var account = await _appCache.GetOrAddAsync(BuildGetAccountCacheKey(request.Allycode),
                () => GetAccount(request.Allycode), 
                TimeSpan.FromDays(30));
                
            if (account is null)
                return ServiceResponse.Fail<AccountDto?>("Account does not exist");
            
            return ServiceResponse.Ok(account)!; 
        }
        
        public async Task<ServiceResponse<AccountDto?>> Handle(GetPrimaryAccountRequest request, CancellationToken cancellationToken)
        {
            var accountExists = await _mediator.Send(new UserExistsRequest(request.DiscordUserId), cancellationToken);

            if (!accountExists)
                ServiceResponse.Fail("User does not exist");

            var account = await _appCache.GetOrAddAsync(BuildGetPrimaryAccountCacheKey(request.DiscordUserId),
                () => GetPrimaryAccount(request.DiscordUserId),
                TimeSpan.FromDays(30));

            if (account is null)
                return ServiceResponse.Fail<AccountDto?>("Account does not exist");
            
            return ServiceResponse.Ok(account)!;
        }

        private async Task<AccountDto?> GetAccount(uint allycode)
        {
            return await _serennoContext.Accounts
                .Where(x => x.Allycode == allycode)
                .Select(x => new AccountDto(x.UserId, x.Allycode, x.AccountName, x.AccountLevel, x.IsPrimaryAccount))
                .SingleOrDefaultAsync();
        }

        private async Task<List<AccountDto?>> GetAllAccounts(ulong discordId)
        {
            return await _serennoContext.Accounts
                .Where(x => x.UserId == discordId)
                .Select(x => new AccountDto(x.UserId, x.Allycode, x.AccountName, x.AccountLevel, x.IsPrimaryAccount))
                .ToListAsync();
        }

        private async Task<AccountDto?> GetPrimaryAccount(ulong discordId)
        {
            return await _serennoContext.Accounts
                .Where(x => x.UserId == discordId)
                .Where(x => x.IsPrimaryAccount)
                .Select(x => new AccountDto(x.UserId, x.Allycode, x.AccountName, x.AccountLevel, x.IsPrimaryAccount))
                .SingleOrDefaultAsync();
        }

        private string BuildGetPrimaryAccountCacheKey(ulong discordId)
        {
            return $"{nameof(AccountHandler)}/{nameof(GetPrimaryAccount)}/{discordId}";
        }
        
        private string BuildGetAccountCacheKey(uint allycode)
        {
            return $"{nameof(AccountHandler)}/{nameof(GetAccount)}/{allycode}";
        }
        
        private string BuildGetAllAccountsCacheKey(ulong discordId)
        {
            return $"{nameof(AccountHandler)}/{nameof(GetAllAccounts)}/{discordId}";
        }
    }
}