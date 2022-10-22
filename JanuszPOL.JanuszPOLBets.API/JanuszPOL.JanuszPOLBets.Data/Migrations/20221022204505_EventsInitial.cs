using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JanuszPOL.JanuszPOLBets.Data.Migrations
{
    public partial class EventsInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Event",
                columns: new[] { "Id", "Description", "EventTypeId", "Name" },
                values: new object[,]
                {
                    { 1L, "Zadecyduj czy mecz zakończy się w doliczonym czasie gry", 1L, "Wygrana w dogrywce" },
                    { 2L, "Zadecyduj czy mecz zakończy się doperio po rzutach karnych", 1L, "Wygrana w karnych" },
                    { 3L, "Zadecyduj ile co najmniej zostanie pokazanych zółtych kartek", 1L, "Ilość żółtych kartek (>=)" },
                    { 4L, "Zadecyduj ile co najmniej padnie bramek", 2L, "Ilość bramek" },
                    { 6L, "Zadecyduj czy bramka padnie do 10 minuty", 1L, "Bramka do 10 minuty" }
                });

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "Type" },
                values: new object[] { 3L, "TwoExactValues" });

            migrationBuilder.InsertData(
                table: "Event",
                columns: new[] { "Id", "Description", "EventTypeId", "Name" },
                values: new object[] { 5L, "Wytypuj konkretny wynik meczu", 3L, "Dokładny wynik" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
