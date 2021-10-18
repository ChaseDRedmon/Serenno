using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Serenno.Domain.Models.Core.Guilds;

[Table(nameof(DiscordAccount), Schema = "Serenno")]
public class DiscordAccount 
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong Id { get; set; }
    public DateTimeOffset JoinedDate { get; set; }
    
    public ulong? GuildId { get; set; }
    public virtual DiscordGuild? Guild { get; set; }
    
    public virtual ICollection<SwgohAccount> Accounts { get; set; }
    public virtual ICollection<Reminder> UserReminders { get; set; }
}