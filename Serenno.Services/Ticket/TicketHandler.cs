using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using MediatR;
using Serenno.Domain;
using Serenno.Domain.Models.Core;
using Serenno.Domain.Models.Core.Ticket;
using Serenno.Services.Accounts;

namespace Serenno.Services.Ticket
{
    public sealed record GetAllTicketsRequest(ulong DiscordUserId) : IRequest<ServiceResponse<List<UserTickets>>>;
    public sealed record GetDailyTicketsRequest(ulong DiscordUserId) : IRequest<ServiceResponse<List<UserTickets>>>;
    public sealed record SetDailyTicketRequest(ulong DiscordUserId, ushort TicketAmount, uint? Allycode) : IRequest { }
    
    public class TicketHandler : 
        AsyncRequestHandler<SetDailyTicketRequest>,
        IRequestHandler<GetAllTicketsRequest, ServiceResponse<List<UserTickets>>>,
        IRequestHandler<GetDailyTicketsRequest, ServiceResponse<List<UserTickets>>>
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

        public Task<ServiceResponse<List<UserTickets>>> Handle(GetDailyTicketsRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
        
        protected override async Task Handle(SetDailyTicketRequest request, CancellationToken cancellationToken)
        {
            // If they do not specify an allycode, use the primary account we have on record
            if (request.Allycode is null)
            {
                // Find the primary account here
                var account = await _mediator.Send(new GetPrimaryAccountRequest(request.DiscordUserId), cancellationToken);
                if (!account.Success)
                    return;

                var primaryAccount = account.Value;
                await SetAccountTicketLevel(primaryAccount.Allycode, request.TicketAmount);
                
            }
            else
            {
                // If we get an allycode code from the command, check to see if 
            }
        }

        private async Task SetAccountTicketLevel(uint allycode, ushort ticketAmount)
        {
            
        }
    }
}