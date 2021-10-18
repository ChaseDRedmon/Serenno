using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serenno.Domain;

namespace Serenno.Services.Users
{
    public class RemoveUserHandler : AsyncRequestHandler<AddUserRequest>
    {
        private readonly SerennoContext _serennoContext;
        private readonly IMediator _mediator;

        public RemoveUserHandler(SerennoContext serennoContext, IMediator mediator)
        {
            _serennoContext = serennoContext;
            _mediator = mediator;
        }

        protected override async Task Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}