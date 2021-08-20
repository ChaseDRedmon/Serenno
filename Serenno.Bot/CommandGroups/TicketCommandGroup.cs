using System;
using System.ComponentModel;
using System.Threading.Tasks;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
using Serenno.Bot.Helpers;
using Serenno.Services.Accounts;

namespace Serenno.Bot.CommandGroups
{
    [Group("ticket")]
    public class TicketCommandGroup : CommandGroup
    {
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _commandContext;
        private readonly IDiscordRestWebhookAPI _discordRestWebhookApi;
        private readonly IMediator _mediator;
        private readonly CommandResponder _commandResponder;

        public TicketCommandGroup(IDiscordRestChannelAPI channelApi, 
            ICommandContext commandContext, 
            IDiscordRestWebhookAPI discordRestWebhookApi,
            IMediator mediator,
            CommandResponder commandResponder)
        {
            _channelApi = channelApi;
            _commandContext = commandContext;
            _discordRestWebhookApi = discordRestWebhookApi;
            _mediator = mediator;
            _commandResponder = commandResponder;
        }
        
        [RequireContext(ChannelContext.Guild), Command("set"), Description("Set your current ticket amount")]
        public async Task<IResult> SetTicketLevel(int tickets)
        {
            var user = _commandContext.User.ID.Value;
            var account = await _mediator.Send(new GetPrimaryAccountRequest(user));

            if (account.Failure)
                return await _commandResponder.Respond("");
            
            throw new NotImplementedException();
        }
        
        [RequireContext(ChannelContext.Guild), Command("set"), Description("Set your current ticket amount")]
        public async Task<IResult> SetAllycodeTicketLevel(int allycode, int tickets)
        {
            //var account = await _mediator.Send(new AllycodeExistsRequest());
            
            
            throw new NotImplementedException();
        }
        
        [RequireContext(ChannelContext.Guild), Command("view"), Description("See your current ticket amount")]
        public async Task<IResult> GetTicketLevel()
        {
            
            throw new NotImplementedException();
        }
    }
}