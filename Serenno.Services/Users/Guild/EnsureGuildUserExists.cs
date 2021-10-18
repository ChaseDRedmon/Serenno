using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serenno.Domain;
using Serenno.Domain.Models.Core.Guilds;
using Serenno.Services.Users.User;

namespace Serenno.Services.Users
{
    public sealed record EnsureGuildUserExistsRequest(ulong DiscordUserId, ulong DiscordGuildId) : IRequest;
    
    public class EnsureGuildUserExistsHandler : 
        AsyncRequestHandler<EnsureGuildUserExistsRequest>
    {
        private readonly SerennoContext _context;
        private readonly IMediator _mediator;

        public EnsureGuildUserExistsHandler(SerennoContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        protected override async Task Handle(EnsureGuildUserExistsRequest request, CancellationToken cancellationToken)
        {
            var userExists = await _mediator.Send(new GuildUserExistsRequest(request.DiscordUserId, request.DiscordGuildId), cancellationToken);

            if (userExists)
                return;

            var user = new User
            {
                Id = request.DiscordUserId,
                JoinedDate = default,
                GuildId = request.DiscordUserId,
                Guild = null,
                Accounts = null,
                UserReminders = null,

            };

            _context.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            await _mediator.Send(new InvalidateUserExistsRequest(request.DiscordUserId, request.DiscordGuildId), cancellationToken);
        }
    }
}