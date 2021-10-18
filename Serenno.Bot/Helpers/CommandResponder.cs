using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace Serenno.Bot.Helpers
{
    public class CommandResponder
    {
        private readonly ICommandContext _commandContext;
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly FeedbackService _feedbackService;

        public CommandResponder(FeedbackService feedbackService, IDiscordRestChannelAPI channelApi, ICommandContext commandContext)
        {
            _feedbackService = feedbackService;
            _channelApi = channelApi;
            _commandContext = commandContext;
        }

        public async Task<IResult> Respond(string message)
        {
            if (_commandContext is InteractionContext interactionContext)
            {
                return await _feedbackService.SendContextualContentAsync(message, Color.Green);
            }

            return await _channelApi.CreateMessageAsync(_commandContext.ChannelID, content: message);
        }

        public async Task<IResult> Respond(Embed embed)
        {
            if (_commandContext is InteractionContext interactionContext)
            {
                return await _feedbackService.SendContextualEmbedAsync(embed);
            }

            return await _channelApi.CreateMessageAsync(_commandContext.ChannelID, embeds: new[] { embed });
        }

        public async Task<IResult> Respond(params Embed[] embeds)
        {
            if (_commandContext is not InteractionContext interactionContext)
                return await _channelApi.CreateMessageAsync(_commandContext.ChannelID, embeds: embeds);
            
            foreach (var embed in embeds)
            {
                await _feedbackService.SendContextualEmbedAsync(embed);
            }

            return Result.FromSuccess();
        }
    }
}