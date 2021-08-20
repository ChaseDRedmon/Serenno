using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Serenno.Domain.Models.Core.Events;
using Serenno.Domain.Models.Core.Ticket;

namespace Serenno.Domain.Models.Core.Guilds
{
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint Allycode { get; set; }
        public string? AccountName { get; set; }
        public byte? AccountLevel { get; set; }
        public bool IsPrimaryAccount { get; set; }        
        public ulong UserId { get; set; }
        public virtual User User { get; set; }
        
        public virtual ICollection<UserTickets> Tickets { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<UserEnergy> Energy { get; set; }
        public virtual ICollection<Reminder> AccountReminders { get; set; }
    }
}