using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Serenno.Domain.Models.Core.Events
{
    [Table(nameof(EventPhase), Schema = "Serenno")]
    public class EventPhase
    {
        public uint Id { get; set; }
        
        [MaxLength(50)]
        public string Phase { get; set; }
        
        public DateTimeOffset PhaseStartTime { get; set; }
        public DateTimeOffset PhaseEndTime { get; set; }
        
        public uint EventId { get; set; }
        public virtual Event Event { get; set; }
        
        public ICollection<Reminder> EventPhaseReminders { get; set; }
    }
}