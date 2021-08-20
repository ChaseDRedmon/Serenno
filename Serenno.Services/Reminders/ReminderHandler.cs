using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using MediatR;
using Serenno.Domain;
using Serenno.Domain.Models.Core;
using Serenno.Services.Users;

namespace Serenno.Services.Reminders
{
    public sealed record GetAllRemindersRequest(ulong DiscordUserId) : IRequest<ServiceResponse<List<Reminder>>>;
    public sealed record SetReminderRequest(ulong DiscordUserId, ulong? allycode) : IRequest { }
    
    public class ReminderHandler : 
        AsyncRequestHandler<SetReminderRequest>,
        IRequestHandler<GetAllRemindersRequest, ServiceResponse<List<Reminder>>>        
    {
        private readonly SerennoContext _context;
        private readonly IAppCache _appCache;
        private readonly IMediator _mediator;

        public ReminderHandler(SerennoContext context, IAppCache appCache, IMediator mediator)
        {
            _context = context;
            _appCache = appCache;
            _mediator = mediator;
        }

        public async Task<ServiceResponse<List<Reminder>>> Handle(GetAllRemindersRequest request, CancellationToken cancellationToken)
        {
            //var reminders = _context.GuildMembers.Select(x => x.);

            throw new NotImplementedException();
        }

        protected override Task Handle(SetReminderRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}