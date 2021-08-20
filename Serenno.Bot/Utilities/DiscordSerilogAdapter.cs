using Microsoft.Extensions.Logging;

namespace Serenno.Bot.Utilities
{
    public class DiscordSerilogAdapter
    {
        private readonly ILogger<DiscordSerilogAdapter> _log;

        public DiscordSerilogAdapter(ILogger<DiscordSerilogAdapter> log)
        {
            _log = log;
        }
    }
}