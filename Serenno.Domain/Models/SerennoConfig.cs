namespace Serenno.Domain.Models;

    public class SerennoConfig
    {
        public string SwgohEventsICALURL { get; set; }
        public string SwgohGGAPIURL { get; set; }
        public string DbConnection { get; set; }
        public string SentryIOToken { get; set; }
        public string DiscordClientId { get; set; }
        public string DiscordClientSecret { get; set; }
        public string DiscordToken { get; set; }
        public ulong DiscordGuildSnowflake { get; set; }
        public int QueueCapacity { get; set; }
    }
