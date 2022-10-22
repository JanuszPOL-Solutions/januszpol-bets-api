using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JanuszPOL.JanuszPOLBets.Data.Migrations
{
    public partial class EventsBaseBet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Result",
                table: "EventBet",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "", "Wygrana pierwszej drużyny" });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "", "Wygrana drugiej drużyny" });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "", "Remis" });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Zadecyduj czy mecz zakończy się w doliczonym czasie gry", "Wygrana w dogrywce" });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "Description", "EventTypeId", "Name" },
                values: new object[] { "Zadecyduj czy mecz zakończy się doperio po rzutach karnych", 2L, "Wygrana w karnych" });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "Description", "EventTypeId", "Name" },
                values: new object[] { "Zadecyduj ile co najmniej zostanie pokazanych zółtych kartek", 2L, "Ilość żółtych kartek (>=)" });

            migrationBuilder.InsertData(
                table: "Event",
                columns: new[] { "Id", "Description", "EventTypeId", "Name" },
                values: new object[,]
                {
                    { 7L, "Zadecyduj ile co najmniej padnie bramek", 3L, "Ilość bramek" },
                    { 9L, "Zadecyduj czy bramka padnie do 10 minuty", 2L, "Bramka do 10 minuty" }
                });

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Type",
                value: "BaseBet");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Type",
                value: "Boolean");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Type",
                value: "ExactValue");

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "Type" },
                values: new object[] { 4L, "TwoExactValues" });

            migrationBuilder.InsertData(
                table: "Event",
                columns: new[] { "Id", "Description", "EventTypeId", "Name" },
                values: new object[] { 8L, "Wytypuj konkretny wynik meczu", 4L, "Dokładny wynik" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.AlterColumn<int>(
                name: "Result",
                table: "EventBet",
                type: "int",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Zadecyduj czy mecz zakończy się w doliczonym czasie gry", "Wygrana w dogrywce" });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Zadecyduj czy mecz zakończy się doperio po rzutach karnych", "Wygrana w karnych" });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Zadecyduj ile co najmniej zostanie pokazanych zółtych kartek", "Ilość żółtych kartek (>=)" });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Zadecyduj ile co najmniej padnie bramek", "Ilość bramek" });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "Description", "EventTypeId", "Name" },
                values: new object[] { "Wytypuj konkretny wynik meczu", 3L, "Dokładny wynik" });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "Description", "EventTypeId", "Name" },
                values: new object[] { "Zadecyduj czy bramka padnie do 10 minuty", 1L, "Bramka do 10 minuty" });

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Type",
                value: "Boolean");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Type",
                value: "ExactValue");

            migrationBuilder.UpdateData(
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Type",
                value: "TwoExactValues");
        }
    }
}
