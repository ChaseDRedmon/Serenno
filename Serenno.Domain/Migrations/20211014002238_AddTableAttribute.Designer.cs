﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Serenno.Domain;

#nullable disable

namespace Serenno.Domain.Migrations
{
    [DbContext(typeof(SerennoContext))]
    [Migration("20211014002238_AddTableAttribute")]
    partial class AddTableAttribute
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0-rc.2.21480.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Serenno.Domain.Models.Core.Events.Event", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("EndDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("EventCategory")
                        .HasColumnType("int");

                    b.Property<decimal?>("GuildId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("GuildId");

                    b.ToTable("Event", "Serenno");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Events.EventPhase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<string>("Phase")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTimeOffset>("PhaseEndTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("PhaseStartTime")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventPhase", "Serenno");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Guilds.Account", b =>
                {
                    b.Property<long>("Allycode")
                        .HasColumnType("bigint");

                    b.Property<byte?>("AccountLevel")
                        .HasColumnType("tinyint");

                    b.Property<string>("AccountName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPrimaryAccount")
                        .HasColumnType("bit");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Allycode");

                    b.HasIndex("UserId");

                    b.ToTable("Account", "Serenno");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Guilds.Guild", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<long>("GuildCode")
                        .HasColumnType("bigint");

                    b.Property<string>("GuildName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Guild", "Serenno");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Guilds.User", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal?>("GuildId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("JoinedDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("User", "Serenno");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Reminder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<TimeSpan>("AlertTime")
                        .HasColumnType("time");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal?>("DiscordUserId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<long?>("EventId")
                        .HasColumnType("bigint");

                    b.Property<long?>("EventPhaseId")
                        .HasColumnType("bigint");

                    b.Property<string>("ReminderName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ReminderTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("ReminderType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("DiscordUserId");

                    b.HasIndex("EventId");

                    b.HasIndex("EventPhaseId");

                    b.ToTable("Reminder", "Serenno");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Ticket.UserTickets", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<long>("AccountAllycode")
                        .HasColumnType("bigint");

                    b.Property<long>("AccountFK")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("TicketAmount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountAllycode");

                    b.ToTable("UserTickets", "Serenno");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.UserEnergy", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<byte>("EnergyAmount")
                        .HasColumnType("tinyint");

                    b.Property<int>("EnergyType")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Time")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("UserEnergy", "Serenno");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Events.Event", b =>
                {
                    b.HasOne("Serenno.Domain.Models.Core.Guilds.Account", "Account")
                        .WithMany("Events")
                        .HasForeignKey("AccountId");

                    b.HasOne("Serenno.Domain.Models.Core.Guilds.Guild", "Guild")
                        .WithMany("Event")
                        .HasForeignKey("GuildId");

                    b.Navigation("Account");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Events.EventPhase", b =>
                {
                    b.HasOne("Serenno.Domain.Models.Core.Events.Event", "Event")
                        .WithMany("EventPhase")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Guilds.Account", b =>
                {
                    b.HasOne("Serenno.Domain.Models.Core.Guilds.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Guilds.User", b =>
                {
                    b.HasOne("Serenno.Domain.Models.Core.Guilds.Guild", "Guild")
                        .WithMany("User")
                        .HasForeignKey("GuildId");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Reminder", b =>
                {
                    b.HasOne("Serenno.Domain.Models.Core.Guilds.Account", "Account")
                        .WithMany("AccountReminders")
                        .HasForeignKey("AccountId");

                    b.HasOne("Serenno.Domain.Models.Core.Guilds.User", "DiscordUser")
                        .WithMany("UserReminders")
                        .HasForeignKey("DiscordUserId");

                    b.HasOne("Serenno.Domain.Models.Core.Events.Event", "Event")
                        .WithMany("EventReminders")
                        .HasForeignKey("EventId");

                    b.HasOne("Serenno.Domain.Models.Core.Events.EventPhase", "EventPhase")
                        .WithMany("EventPhaseReminders")
                        .HasForeignKey("EventPhaseId");

                    b.Navigation("Account");

                    b.Navigation("DiscordUser");

                    b.Navigation("Event");

                    b.Navigation("EventPhase");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Ticket.UserTickets", b =>
                {
                    b.HasOne("Serenno.Domain.Models.Core.Guilds.Account", "Account")
                        .WithMany("Tickets")
                        .HasForeignKey("AccountAllycode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.UserEnergy", b =>
                {
                    b.HasOne("Serenno.Domain.Models.Core.Guilds.Account", "Account")
                        .WithMany("Energy")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Events.Event", b =>
                {
                    b.Navigation("EventPhase");

                    b.Navigation("EventReminders");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Events.EventPhase", b =>
                {
                    b.Navigation("EventPhaseReminders");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Guilds.Account", b =>
                {
                    b.Navigation("AccountReminders");

                    b.Navigation("Energy");

                    b.Navigation("Events");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Guilds.Guild", b =>
                {
                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Serenno.Domain.Models.Core.Guilds.User", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("UserReminders");
                });
#pragma warning restore 612, 618
        }
    }
}
