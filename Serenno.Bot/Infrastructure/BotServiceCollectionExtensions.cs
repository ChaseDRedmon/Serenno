using System.Linq;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;
using Serenno.Bot.CommandGroups;
using Serenno.Domain.Models;

namespace Serenno.Bot.Infrastructure
{
    public static class BotServiceCollectionExtensions
    {
        public static IServiceCollection AddSerennoServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<SerennoConfig>(configuration);

            services
                .AddHangfire(x =>
                    x.UseSqlServerStorage(configuration.GetValue<string>(nameof(SerennoConfig.DbConnection))))
                .AddHangfireServer()
                .AddDiscordGateway(_ => configuration.GetValue<string>(nameof(SerennoConfig.DiscordToken)))
                .Configure<DiscordGatewayClientOptions>(options =>
                {
                    options.Intents |= GatewayIntents.GuildMembers;
                    options.Intents |= GatewayIntents.GuildMessages;
                    options.Intents |= GatewayIntents.GuildMessageReactions;
                })
                .AddDiscordCommands(true)
                .AddCommandGroup<EnergyCommandGroup>()
                .AddCommandGroup<EventCommandGroup>()
                .AddCommandGroup<GuildCommandGroup>()
                .AddCommandGroup<ReminderCommandGroup>()
                .AddCommandGroup<UserCommandGroup>()
                .AddCommandGroup<HelpCommandGroup>()
                .AddCommandGroup<AccountCommandGroup>();

            var responderTypes = typeof(SerennoBot).Assembly
                .GetExportedTypes()
                .Where(t => t.IsResponder());

            foreach (var responder in responderTypes)
            {
                services.AddResponder(responder);
            }

            return services;
        }
    }
}