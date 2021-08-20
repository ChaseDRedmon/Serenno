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
using Serenno.Services.Accounts;

namespace Serenno.Bot.CommandGroups
{
    [Group("account")]
    public class AccountCommandGroup : CommandGroup
    {
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _commandContext;
        private readonly IDiscordRestWebhookAPI _discordRestWebhookApi;
        private readonly IMediator _mediator;

        public AccountCommandGroup(IDiscordRestChannelAPI channelApi, ICommandContext commandContext, IDiscordRestWebhookAPI discordRestWebhookApi, IMediator mediator)
        {
            _channelApi = channelApi;
            _commandContext = commandContext;
            _discordRestWebhookApi = discordRestWebhookApi;
            _mediator = mediator;
        }

        [RequireContext(ChannelContext.Guild), Command("register"), Description("Register your allycode with the bot")]
        public async Task<IResult> RegisterUserAsync(string allyCode, bool isAccountPrimary)
        {
            var cleanedAllyCode = allyCode.Replace("-", string.Empty);
            var success = uint.TryParse(cleanedAllyCode, out var code);

            if (!success)
            {
                var errorEmbed = new Embed(
                    Title: "Invalid Allycode Format",
                    Colour: Color.Red,
                    Description: $"\"{allyCode}\" isn't a valid format for an allycode!",
                    Fields: new List<IEmbedField>
                    {
                        new EmbedField("1) Accepted Format", "123-456-789", IsInline: true),
                        new EmbedField("2) Accepted Format", "123456789", IsInline: true)
                    },
                    Footer: new EmbedFooter("Run \"/help register\" for help"));

                var errorMessageResponse = await Respond(errorEmbed);

                return !errorMessageResponse.IsSuccess
                    ? Result.FromError(errorMessageResponse)
                    : Result.FromSuccess();
            }
            
            var registrationResponse = await _mediator.Send(new RegisterNewAccountRequest(_commandContext.User.ID.Value, code, isAccountPrimary));

            if (registrationResponse.Failure)
            {
                var failureEmbed = new Embed(
                    Title: "Failed to register new account", 
                    EmbedType.Rich, 
                    Description: $"Allycode: {cleanedAllyCode} was not registered",
                    Colour: Color.Red);
                
                var responseFailureResult = await Respond(failureEmbed);
                return Result.FromError(responseFailureResult);
            }
            
            var embed = new Embed(
                Title: "Registration Successful!", 
                EmbedType.Rich, 
                Description: $"Allycode: {code} was registered successfully",
                Colour: Color.Green);
            
            var responseResult = await Respond(embed);
            return !responseResult.IsSuccess
                ? Result.FromError(responseResult)
                : Result.FromSuccess();
        }

        [RequireContext(ChannelContext.Guild), Command("register-alternate"), Description("Register alternate allycodes with your account")]
        public async Task<IResult> RegisterAlternateUserAsync(string allyCode)
        {
            var cleanedAllyCode = allyCode.Replace("-", string.Empty);
            var success = uint.TryParse(cleanedAllyCode, out var code);
            
            if (!success)
            {
                var errorEmbed = new Embed(
                    Title: "Invalid Allycode Format",
                    Colour: Color.Red,
                    Description: $"\"{allyCode}\" isn't a valid format for an allycode!",
                    Fields: new List<IEmbedField>
                    {
                        new EmbedField("1) Accepted Format", "123-456-789", IsInline: true),
                        new EmbedField("2) Accepted Format", "123456789", IsInline: true)
                    },
                    Footer: new EmbedFooter("Run \"/help register\" for help"));

                var errorMessageResponse = await Respond(errorEmbed);

                return !errorMessageResponse.IsSuccess
                    ? Result.FromError(errorMessageResponse)
                    : Result.FromSuccess();
            }
            
            var registrationResponse = await _mediator.Send(new RegisterAlternateAccountRequest(_commandContext.User.ID.Value, code, false));
            
            if (registrationResponse.Failure)
            {
                var failureEmbed = new Embed(
                    Title: "Failed to register new account", 
                    EmbedType.Rich, 
                    Description: $"Allycode: {cleanedAllyCode} was not registered",
                    Colour: Color.Red);
                
                var responseFailureResult = await Respond(failureEmbed);
                return Result.FromError(responseFailureResult);
            }
            
            var embed = new Embed(
                Title: "Registration Successful!", 
                EmbedType.Rich, 
                Description: $"Allycode: {code} was registered successfully",
                Colour: Color.Green);
            var responseResult = await Respond(embed);
            
            return !responseResult.IsSuccess
                ? Result.FromError(responseResult)
                : Result.FromSuccess();
        }

        private async Task<Result<IMessage>> Respond(Embed embed)
        {
            if (_commandContext is InteractionContext interactionContext)
            {
                return await _discordRestWebhookApi.EditOriginalInteractionResponseAsync(interactionContext.ApplicationID, interactionContext.Token, embeds: new[] { embed });
            }

            return await _channelApi.CreateMessageAsync(_commandContext.ChannelID, embeds: new[] {embed});
        }
    }
}