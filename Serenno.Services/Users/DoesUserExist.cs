using System;
using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serenno.Domain;

namespace Serenno.Services.Users
{
    public sealed record UserExistsRequest(ulong DiscordUserId) : IRequest<bool>;
    public sealed record InvalidateUserExistsRequest(ulong DiscordUserId) : IRequest { }

    public class DoesUserExistHandler : 
        RequestHandler<InvalidateUserExistsRequest>,
        IRequestHandler<UserExistsRequest, bool>
    {
        private readonly SerennoContext _serennoContext;
        private readonly IAppCache _appCache;

        public DoesUserExistHandler(SerennoContext serennoContext, IAppCache appCache)
        {
            _serennoContext = serennoContext;
            _appCache = appCache;
        }

        public async Task<bool> Handle(UserExistsRequest request, CancellationToken cancellationToken)
        {
            return await _appCache
                .GetOrAddAsync(GetCacheKey(request.DiscordUserId),
                    () => _serennoContext.GuildMembers.AnyAsync(x => x.Id == request.DiscordUserId, cancellationToken: cancellationToken),
                    DateTimeOffset.Now.AddDays(30));
        }

        protected override void Handle(InvalidateUserExistsRequest request)
        {
            _appCache.Remove(GetCacheKey(request.DiscordUserId));
        }

        private static string GetCacheKey(ulong discordUserId)
        {
            return $"{nameof(EnsureUserExistsHandler)}/{nameof(UserExistsRequest)}/{discordUserId}";
        }
    }
}