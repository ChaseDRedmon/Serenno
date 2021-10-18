using System;
using System.Linq;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;
using Serenno.Bot.CommandGroups;
using Serenno.Bot.Helpers;
using Serenno.Domain.Models;

namespace Serenno.Bot.Infrastructure
{
    public static class BotServiceCollectionExtensions
    {
        public static IServiceCollection AddSerennoServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SerennoConfig>(configuration);

            var mssqlConnectionString = configuration.GetValue<string>(nameof(SerennoConfig.DbConnection));
            services
                .AddHangfire(globalConfiguration =>
                {
                    globalConfiguration
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseSqlServerStorage(mssqlConnectionString, new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.FromMinutes(1),
                            UseRecommendedIsolationLevel = true,
                            UsePageLocksOnDequeue = true,
                            DisableGlobalLocks = true,
                            SchemaName = "Hangfire.Serenno"
                        });
                })
                .AddHangfireServer((provider, options) =>
                {
                    options.ServerTimeout = TimeSpan.FromSeconds(30);
                    options.HeartbeatInterval = TimeSpan.FromSeconds(15);
                    options.SchedulePollingInterval = TimeSpan.FromMinutes(1);
                    options.ServerCheckInterval = TimeSpan.FromMinutes(1);
                    options.CancellationCheckInterval = TimeSpan.FromMinutes(1);
                });
                
           services
               .AddScoped<CommandResponder>()
               .AddDiscordGateway(_ => configuration.GetValue<string>(nameof(SerennoConfig.DiscordToken)))
               .Configure<DiscordGatewayClientOptions>(options =>
               {
                   options.Intents |= GatewayIntents.GuildMembers;
                   options.Intents |= GatewayIntents.GuildMessages;
                   options.Intents |= GatewayIntents.GuildMessageReactions;
                   options.Intents |= GatewayIntents.DirectMessages;
                   options.Intents |= GatewayIntents.DirectMessageReactions;
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