using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Serenno.Domain.Models.Core.Guilds;

namespace Serenno.Domain.Models.Core.Ticket;


    [Table(nameof(UserTickets), Schema = "Serenno")]
    public class UserTickets 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
        
        public ushort TicketAmount { get; set; }
        
        public uint AccountFK { get; set; }
        public virtual SwgohAccount SwgohAccount { get; set; }
    }
