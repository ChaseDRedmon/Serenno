using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serenno.Domain.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Serenno");

            migrationBuilder.CreateTable(
                name: "Guild",
                schema: "Serenno",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    GuildCode = table.Column<long>(type: "bigint", nullable: false),
                    GuildName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guild", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Serenno",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    JoinedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "Serenno",
                        principalTable: "Guild",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Allycode = table.Column<long>(type: "bigint", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    IsPrimaryAccount = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Allycode);
                    table.ForeignKey(
                        name: "FK_Accounts_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Serenno",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                schema: "Serenno",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EventCategory = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Allycode");
                    table.ForeignKey(
                        name: "FK_Event_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "Serenno",
                        principalTable: "Guild",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserEnergy",
                schema: "Serenno",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EnergyType = table.Column<int>(type: "int", nullable: false),
                    EnergyAmount = table.Column<byte>(type: "tinyint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEnergy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEnergy_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Allycode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTickets",
                schema: "Serenno",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TicketAmount = table.Column<int>(type: "int", nullable: false),
                    AccountFK = table.Column<long>(type: "bigint", nullable: false),
                    AccountAllycode = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTickets_Accounts_AccountAllycode",
                        column: x => x.AccountAllycode,
                        principalTable: "Accounts",
                        principalColumn: "Allycode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventPhase",
                schema: "Serenno",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phase = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhaseStartTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PhaseEndTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EventId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPhase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventPhase_Event_EventId",
                        column: x => x.EventId,
                        principalSchema: "Serenno",
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReminderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReminderType = table.Column<int>(type: "int", nullable: false),
                    ReminderTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AlertTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EventId = table.Column<long>(type: "bigint", nullable: true),
                    EventPhaseId = table.Column<long>(type: "bigint", nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    DiscordUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminders_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Allycode");
                    table.ForeignKey(
                        name: "FK_Reminders_Event_EventId",
                        column: x => x.EventId,
                        principalSchema: "Serenno",
                        principalTable: "Event",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reminders_EventPhase_EventPhaseId",
                        column: x => x.EventPhaseId,
                        principalSchema: "Serenno",
                        principalTable: "EventPhase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reminders_User_DiscordUserId",
                        column: x => x.DiscordUserId,
                        principalSchema: "Serenno",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_AccountId",
                schema: "Serenno",
                table: "Event",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_GuildId",
                schema: "Serenno",
                table: "Event",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPhase_EventId",
                schema: "Serenno",
                table: "EventPhase",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_AccountId",
                table: "Reminders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_DiscordUserId",
                table: "Reminders",
                column: "DiscordUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_EventId",
                table: "Reminders",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_EventPhaseId",
                table: "Reminders",
                column: "EventPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_User_GuildId",
                schema: "Serenno",
                table: "User",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEnergy_AccountId",
                schema: "Serenno",
                table: "UserEnergy",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTickets_AccountAllycode",
                schema: "Serenno",
                table: "UserTickets",
                column: "AccountAllycode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "UserEnergy",
                schema: "Serenno");

            migrationBuilder.DropTable(
                name: "UserTickets",
                schema: "Serenno");

            migrationBuilder.DropTable(
                name: "EventPhase",
                schema: "Serenno");

            migrationBuilder.DropTable(
                name: "Event",
                schema: "Serenno");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Serenno");

            migrationBuilder.DropTable(
                name: "Guild",
                schema: "Serenno");
        }
    }
}
