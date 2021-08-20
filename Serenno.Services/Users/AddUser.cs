using System;
using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serenno.Domain;
using Serenno.Domain.Models.Core.Guilds;

namespace Serenno.Services.Users
{
    public sealed record AddUserRequest(ulong DiscordUserId, DateTimeOffset guildJoinDate) : IRequest { }
    
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
            var userExists = await _mediator.Send(new UserExistsRequest(request.DiscordUserId), cancellationToken);

            if (userExists)
                return;

            var user = new User
            {
                Id = request.DiscordUserId,
                JoinedDate = request.guildJoinDate
            };

            _serennoContext.GuildMembers.Add(user);
            await _serennoContext.SaveChangesAsync(cancellationToken);
            await _mediator.Send(new InvalidateUserExistsRequest(request.DiscordUserId), cancellationToken);
        }
    }
}