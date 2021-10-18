using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using Serenno.Services;

namespace Serenno.Bot.Responders
{
    public class MemberJoinLeaveResponder : IResponder<IGuildMemberAdd>, IResponder<IGuildMemberRemove>, IResponder<IGuildCreate>, IResponder<IGuildDelete>
    {
        private readonly IMediator _mediator;
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly IEventQueue _eventQueue;

        public MemberJoinLeaveResponder(IMediator mediator, IDiscordRestChannelAPI channelApi, IEventQueue eventQueue)
        {
            _mediator = mediator;
            _channelApi = channelApi;
            _eventQueue = eventQueue;
        }

        public async Task<Result> RespondAsync(IGuildMemberAdd gatewayEvent, CancellationToken cancellationToken = default)
        {
            if (!gatewayEvent.User.HasValue)
                return Result.FromSuccess();

            var user = gatewayEvent.User.Value;
            await _eventQueue.Queue(new UserJoinedEvent(gatewayEvent.GuildID.Value, user.ID.Value,));
        }

        public Task<Result> RespondAsync(IGuildMemberRemove gatewayEvent, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result> RespondAsync(IGuildCreate gatewayEvent, CancellationToken ct = new CancellationToken())
        {
            
        }

        public async Task<Result> RespondAsync(IGuildDelete gatewayEvent, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}