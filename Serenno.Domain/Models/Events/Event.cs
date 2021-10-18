using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Serenno.Domain.Models.Core.Guilds;

namespace Serenno.Domain.Models.Core.Events;

    [Table(nameof(Event), Schema = "Serenno")]
    public class Event
    {
        public uint Id { get; set; }
        
        public string Summary { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public EventCategory EventCategory { get; set; }
        
        public uint? AccountId { get; set; }
        public virtual SwgohAccount? Account { get; set; }
        
        public ulong? GuildId { get; set; }
        public virtual DiscordGuild DiscordGuild { get; set; }
        
        public virtual ICollection<Reminder>? EventReminders { get; set; }
        public virtual ICollection<EventPhase>? EventPhase { get; set; }
    }
    
    public enum EventCategory
    {
        ResourceEvent,
        HeroicBattle,
        SpecialEvent,
        AssaultBattles,
        OmegaBattles,
        FleetMastery,
        MythicEvent,
        GalacticChallenge,
        GrandArena,
        TerritoryWar,
        TerritoryBattle
    }
