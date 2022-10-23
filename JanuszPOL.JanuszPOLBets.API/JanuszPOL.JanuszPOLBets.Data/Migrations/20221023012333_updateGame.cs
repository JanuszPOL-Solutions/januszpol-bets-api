using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JanuszPOL.JanuszPOLBets.Data.Migrations
{
    public partial class updateGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroupPhaseId",
                table: "Games",
                newName: "PhaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhaseId",
                table: "Games",
                newName: "GroupPhaseId");
        }
    }
}
