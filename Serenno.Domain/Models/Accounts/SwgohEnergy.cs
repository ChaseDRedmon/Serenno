using System;
using System.ComponentModel.DataAnnotations.Schema;
using Serenno.Domain.Models.Core.Guilds;

namespace Serenno.Domain.Models.Core;

    public enum EnergyType
    {
        Normal,
        Cantina,
        Ship,
        Mod,
        War,
        Invalid
    }
    
    [Table(nameof(UserEnergy), Schema = "Serenno")]
    public class UserEnergy
    {
        public uint Id { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset Time { get; set; }
        
        public EnergyType EnergyType { get; set; }
        
        public byte EnergyAmount { get; set; }
        
        public uint AccountId { get; set; }
        public virtual SwgohAccount SwgohAccount { get; set; }
    }
