using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JanuszPOL.JanuszPOLBets.Data.Migrations
{
    public partial class AddGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GamesResults",
                columns: table => new
                {
                    GameResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesResults", x => x.GameResultId);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Team1IdTeamId = table.Column<long>(type: "bigint", nullable: true),
                    Team2IdTeamId = table.Column<long>(type: "bigint", nullable: true),
                    GameDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Team1Score = table.Column<int>(type: "int", nullable: false),
                    Team2Score = table.Column<int>(type: "int", nullable: false),
                    Team1ScoreExtraTime = table.Column<int>(type: "int", nullable: true),
                    Team2ScoreExtraTime = table.Column<int>(type: "int", nullable: true),
                    Team1ScorePenalties = table.Column<int>(type: "int", nullable: true),
                    Team2ScorePenalties = table.Column<int>(type: "int", nullable: true),
                    GameResultId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_GamesResults_GameResultId",
                        column: x => x.GameResultId,
                        principalTable: "GamesResults",
                        principalColumn: "GameResultId");
                    table.ForeignKey(
                        name: "FK_Games_Teams_Team1IdTeamId",
                        column: x => x.Team1IdTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                    table.ForeignKey(
                        name: "FK_Games_Teams_Team2IdTeamId",
                        column: x => x.Team2IdTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameResultId",
                table: "Games",
                column: "GameResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Team1IdTeamId",
                table: "Games",
                column: "Team1IdTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Team2IdTeamId",
                table: "Games",
                column: "Team2IdTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GamesResults_Name",
                table: "GamesResults",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "GamesResults");
        }
    }
}
