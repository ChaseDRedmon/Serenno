using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Serenno.Domain.Models.Core.Guilds
{
    [Table(nameof(User), Schema = "Serenno")]
    public class User 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        public DateTimeOffset JoinedDate { get; set; }
        
        public ulong? GuildId { get; set; }
        public virtual Guild? Guild { get; set; }
        
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Reminder> UserReminders { get; set; }
    }
}