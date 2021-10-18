using Microsoft.EntityFrameworkCore;
using Serenno.Domain.Models.Core;
using Serenno.Domain.Models.Core.Events;
using Serenno.Domain.Models.Core.Guilds;
using Serenno.Domain.Models.Core.Ticket;

namespace Serenno.Domain;

    public class SerennoContext : DbContext
    {
        public SerennoContext(DbContextOptions<SerennoContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SerennoContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventPhase> EventPhases { get; set; }
        public virtual DbSet<UserEnergy> Energies { get; set; }
        public virtual DbSet<DiscordAccount> GuildMembers { get; set; }
        public virtual DbSet<Reminder> Reminders { get; set; }
        public virtual DbSet<UserTickets> UserTickets { get; set; }
        public virtual DbSet<SwgohAccount> Accounts { get; set; }
    }
