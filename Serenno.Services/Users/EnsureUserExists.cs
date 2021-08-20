using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serenno.Domain;
using Serenno.Domain.Models.Core.Guilds;

namespace Serenno.Services.Users
{
    public sealed record EnsureUserExistsRequest(ulong DiscordUserId) : IRequest { }
    
    public class EnsureUserExistsHandler : AsyncRequestHandler<EnsureUserExistsRequest>
    {
        private readonly SerennoContext _context;
        private readonly IMediator _mediator;

        public EnsureUserExistsHandler(SerennoContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        protected override async Task Handle(EnsureUserExistsRequest request, CancellationToken cancellationToken)
        {
            var userExists = await _mediator.Send(new UserExistsRequest(request.DiscordUserId), cancellationToken);

            if (userExists)
                return;

            var user = new User
            {
                Id = request.DiscordUserId,
            };

            _context.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            await _mediator.Send(new InvalidateUserExistsRequest(request.DiscordUserId), cancellationToken);
        }
    }
}