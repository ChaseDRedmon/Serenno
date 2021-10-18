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
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;
using Serenno.Services.Accounts;
using Serilog;

namespace Serenno.Bot.CommandGroups
{
    [Group("account")]
    public class AccountCommandGroup : CommandGroup
    {
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly ICommandContext _commandContext;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly FeedbackService _feedbackService;

        public AccountCommandGroup(IDiscordRestChannelAPI channelApi, ICommandContext commandContext, IDiscordRestWebhookAPI discordRestWebhookApi, IMediator mediator, ILogger logger, FeedbackService feedbackService)
        {
            _channelApi = channelApi;
            _commandContext = commandContext;
            _mediator = mediator;
            _logger = logger;
            _feedbackService = feedbackService;
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
            
            // TODO: When this method is called, we need to invalidate the cache that Serenno.Services.Accounts.GetAllRegisteredAccountsRequest uses
            var registrationResponse = await _mediator.Send(new RegisterNewAccountRequest(_commandContext.User.ID.Value, _commandContext.GuildID.Value.Value, code, isAccountPrimary));

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

        [RequireContext(ChannelContext.Guild), Command("register-alternate"), Description("Register an alternate, non-primary allycode with your discord account")]
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
            
            var registrationResponse = await _mediator.Send(new RegisterAlternateAccountRequest(_commandContext.User.ID.Value, _commandContext.GuildID.Value.Value, code, false));
            
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

        [RequireContext(ChannelContext.Guild), Command("list"), Description("List all allycodes associated with your account")]
        public async Task<IResult> ListAllAccounts()
        {
            _logger.Information("Listing accounts...");
            
            var user = _commandContext.User.ID.Value;
            var allAssociatedAccounts = await _mediator.Send(new GetAllRegisteredAccountsRequest(user));

            if (allAssociatedAccounts.Failure)
            {
                _logger.Error("Failed to read registered accounts: {Error}", allAssociatedAccounts.ErrorMessage);
                
                var failureEmbed = new Embed(
                    Title: "Failed to read registered accounts", 
                    EmbedType.Rich, 
                    Description: allAssociatedAccounts.ErrorMessage,
                    Colour: Color.Red);
                
                var responseFailureResult = await Respond(failureEmbed);
                return Result.FromError(responseFailureResult);
            }
            
            _logger.Debug("Accounts found");
            var associatedAccounts = allAssociatedAccounts.Value;
            var embedFields = new List<EmbedField>();
            
            _logger.Debug("Associated accounts: {accounts}", associatedAccounts);
            
            _logger.Debug("Adding embeds");
            foreach (var account in associatedAccounts)
            {
                embedFields.Add(new EmbedField(account?.AccountName ?? "Debug Value", $"Allycode: {account.Allycode}\nIs Primary Account?: {account.IsPrimaryAccount}", true));
            }
            
            var embed = new Embed(
                Title: "Registered accounts", 
                Fields: embedFields,
                Colour: Color.Green);
            
            _logger.Debug("Sending response");
            var responseResult = await Respond(embed);
            return !responseResult.IsSuccess
                ? Result.FromError(responseResult)
                : Result.FromSuccess();
        }

        private async Task<Result<IMessage>> Respond(Embed embed)
        {
            _logger.Debug("Entering send method");
            
            if (_commandContext is InteractionContext interactionContext)
            {
                _logger.Debug("Contextual message");
                return await _feedbackService.SendContextualEmbedAsync(embed);
            }
            
            _logger.Debug("Non-contextual message");
            return await _channelApi.CreateMessageAsync(_commandContext.ChannelID, embeds: new[] { embed });
        }
    }
}