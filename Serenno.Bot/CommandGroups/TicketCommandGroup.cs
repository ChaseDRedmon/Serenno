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
using Serenno.Bot.Helpers;
using Serenno.Services.Accounts;
using Serenno.Services.Ticket;

namespace Serenno.Bot.CommandGroups
{
    [Group("ticket")]
    public class TicketCommandGroup : CommandGroup
    {
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _commandContext;
        private readonly IMediator _mediator;
        private readonly CommandResponder _commandResponder;

        public TicketCommandGroup(IDiscordRestChannelAPI channelApi, 
            ICommandContext commandContext, 
            IMediator mediator,
            CommandResponder commandResponder)
        {
            _channelApi = channelApi;
            _commandContext = commandContext;
            _mediator = mediator;
            _commandResponder = commandResponder;
        }
        
        [RequireContext(ChannelContext.Guild), Command("set"), Description("Set your current ticket amount")]
        public async Task<IResult> SetTicketLevel(ushort tickets)
        {
            // TODO: This is probably broken. I probably need to pass the user discrim instead; not sure if command context carries
            await SetAllycodeTicketLevel(tickets);
            return Result.FromSuccess();
        }
        
        [RequireContext(ChannelContext.Guild), Command("set-ally"), Description("Set your current ticket amount")]
        public async Task<IResult> SetAllycodeTicketLevel(ushort tickets, uint? allycode = null)
        {
            var user = _commandContext.User.ID.Value;
            var response = await _mediator.Send(new SetDailyTicketRequest(user, tickets, allycode));
            
            if (response.Failure)
            {
                var errorEmbed = BuildGenericErrorEmbed("Failed to set ticket level", response.ErrorMessage);
                await _commandResponder.Respond(errorEmbed);
                return Result.FromSuccess();
            }

            await _commandResponder.Respond("Ticket level successfully set for primary account");
            return Result.FromSuccess();
        }
        
        [RequireContext(ChannelContext.Guild), Command("view"), Description("See your current ticket amount")]
        public async Task<IResult> GetTicketLevel(uint? allycode = null)
        {
            var user = _commandContext.User.ID.Value;
            var response = await _mediator.Send(new GetDailyTicketsRequest(user, allycode));

            if (response.Failure)
            {
                var errorEmbed = BuildGenericErrorEmbed("Failed to read ticket level", response.ErrorMessage);
                await _commandResponder.Respond(errorEmbed);
                return Result.FromSuccess();
            }

            await _commandResponder.Respond();
            return Result.FromSuccess();
        }

        private Embed BuildSuccessEmbed(List<EmbedField> embedFields)
        {
            return new Embed(
                Title: "Ticket amount per account",
                Type: EmbedType.Rich,
                Fields: embedFields,
                Colour: Color.Green,
                Description: $"Shows current ticket amounts per account",
                Footer: new EmbedFooter("Run \"/help ticket\" for help"));
        }

        private Embed BuildGenericErrorEmbed(string title, string message)
        {
            return new Embed(title, Colour: Color.Red, Description: message);
        }
    }
}