using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JanuszPOL.JanuszPOLBets.Data.Migrations
{
    public partial class PhaseAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Phase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phase", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Phase",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "Group" });

            migrationBuilder.InsertData(
                table: "Phase",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Playoffs" });

            migrationBuilder.CreateIndex(
                name: "IX_Phase_Name",
                table: "Phase",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Phase");
        }
    }
}
