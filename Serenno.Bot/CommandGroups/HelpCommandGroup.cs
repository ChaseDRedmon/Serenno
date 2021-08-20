using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Results;

namespace Serenno.Bot.CommandGroups
{
    [Group("help")]
    public class HelpCommandGroup : CommandGroup
    {
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _commandContext;
        private readonly IDiscordRestWebhookAPI _discordRestWebhookApi;
        private readonly IMediator _mediator;

        public HelpCommandGroup(
            IDiscordRestChannelAPI channelApi,
            ICommandContext commandContext, 
            IDiscordRestWebhookAPI discordRestWebhookApi,
            IMediator mediator)
        {
            _channelApi = channelApi;
            _commandContext = commandContext;
            _discordRestWebhookApi = discordRestWebhookApi;
            _mediator = mediator;
        }
        
        [RequireContext(ChannelContext.Guild), Command("energy"),  Description("Shows a description of the energy commands")]
        public async Task<IResult> DisplayEnergyHelpDocumentation()
        {
            var embed = new Embed("Energy Commands", EmbedType.Rich, Colour: Color.Blue, Fields: new List<EmbedField>
            {
                new("energy set", "Takes two arguments: energy type & energy amount"),
                new("energy view", "Displays the approximate in-game energy levels, since the last time you used \"energy set\"")
            });

            if (_commandContext is not InteractionContext interactionContext)
                return await _channelApi.CreateMessageAsync(_commandContext.ChannelID, embeds: new[] { embed });
            
            var result = await _discordRestWebhookApi.EditOriginalInteractionResponseAsync(interactionContext.ApplicationID, interactionContext.Token, embeds: new[] { embed });
            return !result.IsSuccess
                ? Result.FromError(result)
                : Result.FromSuccess();
        }
        
        [RequireContext(ChannelContext.Guild), Command("register"),  Description("Shows a description of the register commands")]
        public async Task<IResult> DisplayRegisterHelpDocumentation()
        {
            var embed = new Embed("Register Commands", EmbedType.Rich, Colour: Color.Blue, Fields: new List<EmbedField>
            {
                new(@"1) /register user", "Takes one argument which must be a valid SWGOH AllyCode\nValid Formats: 123-456-789 or 123456789"),
            });

            if (_commandContext is not InteractionContext interactionContext)
                return await _channelApi.CreateMessageAsync(_commandContext.ChannelID, embeds: new[] { embed });
            
            var result = await _discordRestWebhookApi.EditOriginalInteractionResponseAsync(interactionContext.ApplicationID, interactionContext.Token, embeds: new[] { embed });
            return !result.IsSuccess
                ? Result.FromError(result)
                : Result.FromSuccess();
        }
    }
}