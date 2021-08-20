using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Remora.Discord.Commands.Services;
using Remora.Discord.Core;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Results;
using Remora.Results;
using Serenno.Domain.Models;
using Serilog;

namespace Serenno.Bot
{
    public sealed class SerennoBot : BackgroundService
    {
        private readonly DiscordGatewayClient _client;
        private readonly SlashService _slashService;
        private readonly SerennoConfig _options;
        private readonly ILogger _log;
        
        public SerennoBot(DiscordGatewayClient client, SlashService slashService, IOptions<SerennoConfig> options, ILogger logger)
        {
            _client = client;
            _slashService = slashService;
            _options = options.Value;
            _log = logger.ForContext<SerennoBot>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _log.Information("Starting bot background service");

            await InitialiseSlashCommands(stoppingToken);
            
            var runResult = await _client.RunAsync(stoppingToken);
            
            if (!runResult.IsSuccess)
            {
                switch (runResult.Error)
                {
                    case ExceptionError exe:
                    {
                        _log.Error
                        (
                            exe.Exception,
                            "Exception during gateway connection: {ExceptionMessage}",
                            exe.Message
                        );

                        break;
                    }
                    case GatewayWebSocketError:
                    case GatewayDiscordError:
                    {
                        _log.Error("Gateway error: {Message}", runResult.Error?.Message);
                        break;
                    }
                    default:
                    {
                        _log.Error("Unknown error: {Message}", runResult.Error?.Message);
                        break;
                    }
                }
            }

            _log.Information("Bye bye");
        }
        
        private async Task InitialiseSlashCommands(CancellationToken cancellationToken)
        {
            var slashSupport = _slashService.SupportsSlashCommands();

            if (!slashSupport.IsSuccess)
            {
                _log.Warning("The registered commands of the bot don't support slash commands: {Reason}", slashSupport.Error?.Message);
            }
            else
            {
                var updateSlash = await _slashService.UpdateSlashCommandsAsync(new Snowflake(_options.DiscordGuildSnowflake), ct: cancellationToken);

                if (!updateSlash.IsSuccess)
                {
                    _log.Warning("Failed to update slash commands: {Reason}", updateSlash.Error?.Message);
                }
            }
        }
    }
}