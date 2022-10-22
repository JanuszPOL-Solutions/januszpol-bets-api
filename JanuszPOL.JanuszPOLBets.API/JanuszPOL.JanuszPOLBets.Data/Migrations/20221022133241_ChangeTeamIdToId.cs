using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JanuszPOL.JanuszPOLBets.Data.Migrations
{
    public partial class ChangeTeamIdToId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Teams",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Teams",
                newName: "TeamId");
        }
    }
}
