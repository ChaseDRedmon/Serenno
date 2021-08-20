using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Core;
using Remora.Results;
using Serenno.Bot.Helpers;
using Serenno.Domain.Models.Core;
using Serenno.Services.Energy;

namespace Serenno.Bot.CommandGroups
{
    [Group("energy")]
    public class EnergyCommandGroup : CommandGroup
    {
        private readonly ICommandContext _commandContext;
        private readonly IMediator _mediator;
        private readonly CommandResponder _commandResponder;

        public EnergyCommandGroup(
            ICommandContext commandContext, 
            IMediator mediator,
            CommandResponder commandResponder)
        {
            _commandContext = commandContext;
            _mediator = mediator;
            _commandResponder = commandResponder;
        }

        [RequireContext(ChannelContext.Guild), Command("set"), Description("Set your current energy level")]
        public async Task<IResult> SetEnergyLevel(byte? warEnergy, byte? shipEnergy, byte? modEnergy, byte? cantinaEnergy,
            byte? normalEnergy)
        {
            var isEverythingNull = AreAllNull(warEnergy, shipEnergy, modEnergy, cantinaEnergy, normalEnergy);

            if (isEverythingNull)
                return await _commandResponder.Respond("You must include a value in one of the parameters!");
            
            var user = _commandContext.User.ID.Value;

            var bulkRequests = new List<EnergyDto>
            {
                new(EnergyType.War, warEnergy),
                new(EnergyType.Ship, shipEnergy),
                new(EnergyType.Mod, modEnergy),
                new(EnergyType.Cantina, cantinaEnergy),
                new(EnergyType.Normal, normalEnergy)
            };

            var result = await _mediator.Send(new SetBulkEnergyLevelRequest(user, bulkRequests));

            if (result.Failure)
            {
                var errorEmbed = BuildGenericErrorEmbed("Failed to set energy levels", $"Error Message: \n{result.Failure}");
                return await _commandResponder.Respond(errorEmbed);
            }

            await _commandResponder.Respond("Successfully set energy levels");
            return Result.FromSuccess();
            
            static bool AreAllNull(params byte?[] elements) => elements.Any(c => c is null);
        }

        [RequireContext(ChannelContext.Guild), Command("set"), Description("Set your current energy level")]
        public async Task<IResult> SetEnergyLevel(EnergyType energyType, byte energyAmount)
        {
            if (energyAmount is < 0 or > 144)
            {
                var error = BuildInvalidEnergyAmountEmbed(energyAmount);
                await _commandResponder.Respond(error);
                return Result.FromSuccess();
            }

            if (energyType == EnergyType.Invalid)
            {
                var error = BuildInvalidEnergyTypeEmbed(energyType);
                await _commandResponder.Respond(error);
                return Result.FromSuccess();
            }

            var energyRequest = await _mediator.Send(new SetEnergyLevelRequest(_commandContext.User.ID.Value, new EnergyDto(energyType, energyAmount)));

            if (energyRequest.Failure)
            {
                var error = BuildGenericErrorEmbed("Failed to set energy levels", energyRequest.ErrorMessage);
                await _commandResponder.Respond(error);
                return Result.FromSuccess();
            }
            
            var embed = new Embed(
                Title: "Energy level set successfully",
                Colour: Color.Green,
                Description: $"Set {energyType} to {energyAmount} at {DateTime.UtcNow} UTC",
                Footer: new EmbedFooter("Run \"/help energy\" for help"));

            var success = BuildGenericErrorEmbed("Failed to set energy levels", energyRequest.ErrorMessage);
            await _commandResponder.Respond(success);
            return Result.FromSuccess();
        }

        [RequireContext(ChannelContext.Guild), Command("view"), Description("See your approximate in-game energy level")]
        public async Task<IResult> ViewEnergyLevel(string value)
        {
            var request = await _mediator.Send(new GetAllEnergyLevelsRequest(_commandContext.User.ID.Value));

            if (request.Failure)
            {
                var errorEmbed = BuildGenericErrorEmbed("Failed to read energy levels", request.ErrorMessage);
                await _commandResponder.Respond(errorEmbed);
            }

            var energyList = request.Value;

            var embedFields = new List<IEmbedField>();
            foreach (var energy in energyList)
            {
                var minuteDivisor = energy.EnergyType switch
                {
                    EnergyType.Cantina => 12,
                    EnergyType.Normal => 6,
                    EnergyType.Ship => 6,
                    EnergyType.Mod => 6,
                    EnergyType.War => 6
                };

                var approximateEnergyAmount =
                    ((DateTimeOffset.Now - energy.Time).Minutes / minuteDivisor) + energy.EnergyAmount;
                
                embedFields.Add(new EmbedField($"{energy.EnergyType.ToString()} energy: ", approximateEnergyAmount.ToString()));
            }
            
            var energyEmbed = new Embed(
                Title: "Approximate in-game energy levels",
                Type: EmbedType.Rich,
                Fields: embedFields,
                Colour: Color.Green,
                Description: $"Shows approximate in-game energy levels based on last entry from /energy set command",
                Footer: new EmbedFooter("Run \"/help energy\" for help"));

            await _commandResponder.Respond(energyEmbed);
            return Result.FromSuccess();
        }

        private EnergyType ParseEnergyType(string value)
        {
            return value switch
            {
                "Normal" => EnergyType.Normal,
                "Mod" => EnergyType.Mod,
                "Ship" => EnergyType.Ship,
                "Cantina" => EnergyType.Cantina,
                "War" => EnergyType.War,
                _ => EnergyType.Invalid
            };
        }

        private Embed BuildInvalidEnergyAmountEmbed(byte energyLevel)
        {
            var errorEmbed = new Embed(
                Title: "Invalid Energy Level",
                Colour: Color.Red,
                Description: $"\"{energyLevel}\" must be between 0 and 144!",
                Footer: new EmbedFooter("Run \"/help energy\" for help"));

            return errorEmbed;
        }

        private Embed BuildInvalidEnergyTypeEmbed(EnergyType energyType)
        {
            var errorEmbed = new Embed("Invalid Energy Type Argument", Colour: Color.Red,
                Description: $"\"{energyType}\" isn't a valid Energy Type!", Footer: new EmbedFooter("Run \"/help energy\" for help"));

            return errorEmbed;
        }

        private Embed BuildGenericErrorEmbed(string title, string message)
        {
            return new Embed(title, Colour: Color.Red, Description: message);
        }
    }
}