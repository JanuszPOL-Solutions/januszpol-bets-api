using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JanuszPOL.JanuszPOLBets.Data.Migrations
{
    public partial class TestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastLoginDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1L, 0, "34e4522e-db31-401c-9430-c1227d489d39", "mymail@mail123fszd.com", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, null, "123", null, false, null, false, "Testing" });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "FlagUrl", "Name" },
                values: new object[] { 1L, "not used now", "MyTeam1" });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "FlagUrl", "Name" },
                values: new object[] { 2L, "not used now", "MyTeam2" });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "GameDate", "GameResultId", "Team1Id", "Team1Score", "Team1ScoreExtraTime", "Team1ScorePenalties", "Team2Id", "Team2Score", "Team2ScoreExtraTime", "Team2ScorePenalties" },
                values: new object[] { 1L, new DateTime(2022, 10, 23, 22, 48, 3, 147, DateTimeKind.Utc).AddTicks(7276), 0, 1L, null, null, null, 2L, null, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
