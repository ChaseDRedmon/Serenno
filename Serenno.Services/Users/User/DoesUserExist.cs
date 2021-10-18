using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serenno.Domain;
using Serilog;

namespace Serenno.Services.Users.User;

public sealed record UserExistsRequest(ulong DiscordUserId) : IRequest<bool>;
public sealed record GuildUserExistsRequest(ulong DiscordUserId, ulong DiscordGuildId) : IRequest<bool>;
public sealed record InvalidateUserExistsRequest(ulong DiscordUserId, ulong DiscordGuildId) : IRequest;

public class DoesUserExistHandler : 
    RequestHandler<InvalidateUserExistsRequest>,
    IRequestHandler<UserExistsRequest, bool>
{
    private readonly SerennoContext _serennoContext;
    private readonly IAppCache _appCache;
    private readonly ILogger _logger;

    public DoesUserExistHandler(SerennoContext serennoContext, IAppCache appCache, ILogger logger)
    {
        _serennoContext = serennoContext;
        _appCache = appCache;
        _logger = logger;
    }

    public async Task<bool> Handle(UserExistsRequest request, CancellationToken cancellationToken)
    {
        
    }
    
    public async Task<bool> Handle(GuildUserExistsRequest request, CancellationToken cancellationToken)
    {
        _logger.Debug("Checking if user exists...");
        
        var result = await _appCache
            .GetOrAddAsync(GetCacheKey(request.DiscordUserId, request.DiscordGuildId),
                () => GetUser(request.DiscordUserId, request.DiscordGuildId, cancellationToken),
                DateTimeOffset.Now.AddDays(30));
        
        _logger.Debug("Does user exist?: {Exists}", result);
        return result;
    }

    protected override void Handle(InvalidateUserExistsRequest request)
    {
        _appCache.Remove(GetCacheKey(request.DiscordUserId, request.DiscordGuildId));
    }

    private static string GetCacheKey(ulong discordUserId, ulong discordGuildId)
    {
        return $"{nameof(UserExistsRequest)}/{discordUserId}/{discordGuildId}";
    }

    private async Task<bool> GetUser(ulong DiscordUserId, ulong DiscordGuildId, CancellationToken cancellationToken = default)
    {
        return await _serennoContext.GuildMembers
            .Where(x => x.Id == DiscordUserId)
            .Where(x => x.GuildId == DiscordGuildId)
            .AnyAsync(cancellationToken: cancellationToken);
    }
}
