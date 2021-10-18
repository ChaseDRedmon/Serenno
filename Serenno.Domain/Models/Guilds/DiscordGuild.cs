using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Serenno.Domain.Models.Core.Events;

namespace Serenno.Domain.Models.Core.Guilds;

[Table(nameof(DiscordGuild), Schema = "Serenno")]
public class DiscordGuild 
{
    // Discord guild snowflake
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong Id { get; set; }
    
    // The swgoh guild code 
    public uint GuildCode { get; set; }
    
    [MaxLength(100)]
    public string GuildName { get; set; }
    
    public virtual ICollection<DiscordAccount>? User { get; set; }
    public virtual ICollection<Event>? Event { get; set; }
}