using System;
using Serenno.Domain.Models.Core.Events;
using Serenno.Domain.Models.Core.Guilds;

namespace Serenno.Domain.Models.Core
{
    public enum ReminderType
    {
        GrandArena,
        Events,
        TerritoryWar,
        TerritoryBattle,
        DailyTickets,
        Generic
    }

    public class Reminder 
    {
        public uint Id { get; set; }
        public string? ReminderName { get; set; }
        public ReminderType ReminderType { get; set; }
        public DateTimeOffset ReminderTime { get; set; }
        public DateTimeOffset Created { get; set; }
        public TimeSpan AlertTime { get; set; }
        
        public uint? EventId { get; set; }
        public Event? Event { get; set; }
        
        public uint? EventPhaseId { get; set; }
        public EventPhase? EventPhase { get; set; }
        
        public uint? AccountId { get; set; }
        public Account? Account { get; set; }
        
        public ulong? DiscordUserId { get; set; }
        public User? DiscordUser { get; set; }
    }
}