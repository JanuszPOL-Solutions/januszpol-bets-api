using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JanuszPOL.JanuszPOLBets.Data.Migrations
{
    public partial class GameTableAddPhase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupPhaseId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhaseName",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupPhaseId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PhaseName",
                table: "Games");
        }
    }
}
