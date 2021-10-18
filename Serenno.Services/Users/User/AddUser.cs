using System;
using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serenno.Domain;
using Serenno.Domain.Models.Core.Guilds;
using Serenno.Services.Users.User;

namespace Serenno.Services.Users;

public sealed record AddUserRequest(ulong DiscordUserId, DateTimeOffset guildJoinDate, ulong DiscordGuildId) : IRequest { }

public class AddUserHandler : AsyncRequestHandler<AddUserRequest>
{
    private readonly SerennoContext _serennoContext;
    private readonly IMediator _mediator;

    public AddUserHandler(SerennoContext serennoContext, IMediator mediator)
    {
        _serennoContext = serennoContext;
        _mediator = mediator;
    }
    
    protected override async Task Handle(AddUserRequest request, CancellationToken cancellationToken)
    {
        var userExists = await _mediator.Send(new GuildUserExistsRequest(request.DiscordUserId, request.DiscordGuildId), cancellationToken);

        if (userExists)
            return;

        var user = new Domain.Models.Core.Guilds.DiscordAccount
        {
            Id = request.DiscordUserId,
            JoinedDate = request.guildJoinDate,
            GuildId = request.DiscordGuildId
        };

        _serennoContext.GuildMembers.Add(user);
        await _serennoContext.SaveChangesAsync(cancellationToken);
        await _mediator.Send(new InvalidateUserExistsRequest(request.DiscordUserId, request.DiscordGuildId), cancellationToken);
    }
}
