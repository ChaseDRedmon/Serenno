using System;
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
    [Group("reminder")]
    public class ReminderCommandGroup : CommandGroup
    {
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _commandContext;
        private readonly IMediator _mediator;

        public ReminderCommandGroup(IDiscordRestChannelAPI channelApi, ICommandContext commandContext, IMediator mediator)
        {
            _channelApi = channelApi;
            _commandContext = commandContext;
            _mediator = mediator;
        }
        
        [RequireContext(ChannelContext.Guild), Command("create"), Description("Create a reminder")]
        public async Task<IResult> CreateReminderAsync(string value)
        {
            var rollRequests = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (rollRequests.Length == 0)
            {
                return Result.FromSuccess();
            }

            var fields = new List<IEmbedField>();
            fields.Add(new EmbedField("Field name", "This is my value"));
            
            var embed = new Embed("Reminder", Fields: fields, Colour: Color.Green);

            var reply = await _channelApi.CreateMessageAsync(_commandContext.ChannelID, embeds: new[] {embed});
            
            return !reply.IsSuccess
                ? Result.FromError(reply)
                : Result.FromSuccess();
        }
    }
}