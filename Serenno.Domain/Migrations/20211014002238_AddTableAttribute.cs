using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serenno.Domain.Migrations
{
    public partial class AddTableAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_User_UserId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Accounts_AccountId",
                schema: "Serenno",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Accounts_AccountId",
                table: "Reminders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Event_EventId",
                table: "Reminders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_EventPhase_EventPhaseId",
                table: "Reminders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_User_DiscordUserId",
                table: "Reminders");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEnergy_Accounts_AccountId",
                schema: "Serenno",
                table: "UserEnergy");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTickets_Accounts_AccountAllycode",
                schema: "Serenno",
                table: "UserTickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reminders",
                table: "Reminders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Reminders",
                newName: "Reminder",
                newSchema: "Serenno");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account",
                newSchema: "Serenno");

            migrationBuilder.RenameIndex(
                name: "IX_Reminders_EventPhaseId",
                schema: "Serenno",
                table: "Reminder",
                newName: "IX_Reminder_EventPhaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Reminders_EventId",
                schema: "Serenno",
                table: "Reminder",
                newName: "IX_Reminder_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Reminders_DiscordUserId",
                schema: "Serenno",
                table: "Reminder",
                newName: "IX_Reminder_DiscordUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reminders_AccountId",
                schema: "Serenno",
                table: "Reminder",
                newName: "IX_Reminder_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_UserId",
                schema: "Serenno",
                table: "Account",
                newName: "IX_Account_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reminder",
                schema: "Serenno",
                table: "Reminder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                schema: "Serenno",
                table: "Account",
                column: "Allycode");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_User_UserId",
                schema: "Serenno",
                table: "Account",
                column: "UserId",
                principalSchema: "Serenno",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Account_AccountId",
                schema: "Serenno",
                table: "Event",
                column: "AccountId",
                principalSchema: "Serenno",
                principalTable: "Account",
                principalColumn: "Allycode");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminder_Account_AccountId",
                schema: "Serenno",
                table: "Reminder",
                column: "AccountId",
                principalSchema: "Serenno",
                principalTable: "Account",
                principalColumn: "Allycode");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminder_Event_EventId",
                schema: "Serenno",
                table: "Reminder",
                column: "EventId",
                principalSchema: "Serenno",
                principalTable: "Event",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminder_EventPhase_EventPhaseId",
                schema: "Serenno",
                table: "Reminder",
                column: "EventPhaseId",
                principalSchema: "Serenno",
                principalTable: "EventPhase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminder_User_DiscordUserId",
                schema: "Serenno",
                table: "Reminder",
                column: "DiscordUserId",
                principalSchema: "Serenno",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEnergy_Account_AccountId",
                schema: "Serenno",
                table: "UserEnergy",
                column: "AccountId",
                principalSchema: "Serenno",
                principalTable: "Account",
                principalColumn: "Allycode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTickets_Account_AccountAllycode",
                schema: "Serenno",
                table: "UserTickets",
                column: "AccountAllycode",
                principalSchema: "Serenno",
                principalTable: "Account",
                principalColumn: "Allycode",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_User_UserId",
                schema: "Serenno",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Account_AccountId",
                schema: "Serenno",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminder_Account_AccountId",
                schema: "Serenno",
                table: "Reminder");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminder_Event_EventId",
                schema: "Serenno",
                table: "Reminder");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminder_EventPhase_EventPhaseId",
                schema: "Serenno",
                table: "Reminder");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminder_User_DiscordUserId",
                schema: "Serenno",
                table: "Reminder");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEnergy_Account_AccountId",
                schema: "Serenno",
                table: "UserEnergy");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTickets_Account_AccountAllycode",
                schema: "Serenno",
                table: "UserTickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reminder",
                schema: "Serenno",
                table: "Reminder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                schema: "Serenno",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Reminder",
                schema: "Serenno",
                newName: "Reminders");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "Serenno",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_Reminder_EventPhaseId",
                table: "Reminders",
                newName: "IX_Reminders_EventPhaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Reminder_EventId",
                table: "Reminders",
                newName: "IX_Reminders_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Reminder_DiscordUserId",
                table: "Reminders",
                newName: "IX_Reminders_DiscordUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reminder_AccountId",
                table: "Reminders",
                newName: "IX_Reminders_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_UserId",
                table: "Accounts",
                newName: "IX_Accounts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reminders",
                table: "Reminders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Allycode");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_User_UserId",
                table: "Accounts",
                column: "UserId",
                principalSchema: "Serenno",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Accounts_AccountId",
                schema: "Serenno",
                table: "Event",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Allycode");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Accounts_AccountId",
                table: "Reminders",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Allycode");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Event_EventId",
                table: "Reminders",
                column: "EventId",
                principalSchema: "Serenno",
                principalTable: "Event",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_EventPhase_EventPhaseId",
                table: "Reminders",
                column: "EventPhaseId",
                principalSchema: "Serenno",
                principalTable: "EventPhase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_User_DiscordUserId",
                table: "Reminders",
                column: "DiscordUserId",
                principalSchema: "Serenno",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEnergy_Accounts_AccountId",
                schema: "Serenno",
                table: "UserEnergy",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Allycode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTickets_Accounts_AccountAllycode",
                schema: "Serenno",
                table: "UserTickets",
                column: "AccountAllycode",
                principalTable: "Accounts",
                principalColumn: "Allycode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
