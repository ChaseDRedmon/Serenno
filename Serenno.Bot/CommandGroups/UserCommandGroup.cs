using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Contexts;
using Serenno.Domain;

namespace Serenno.Bot.CommandGroups
{
    [Group("user")]
    public class UserCommandGroup : CommandGroup
    {
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _commandContext;
        private readonly IMediator _mediator;

        public UserCommandGroup(IDiscordRestChannelAPI channelApi, ICommandContext commandContext, IMediator mediator)
        {
            _channelApi = channelApi;
            _commandContext = commandContext;
            _mediator = mediator;
        }
    }
}