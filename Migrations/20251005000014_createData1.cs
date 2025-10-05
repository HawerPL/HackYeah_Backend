using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HackYeah_Backend.Migrations
{
    /// <inheritdoc />
    public partial class createData1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rcbs",
                columns: new[] { "Id", "ArticleUrl", "Description", "EventDate", "ImageRegionUrl", "ImageUrl", "Title" },
                values: new object[,]
                {
                    { 1, "https://www.gov.pl/web/rcb/alert-rcb---szczepionka-na-lisy-przeciw-wsciekliznie-w-wojewodztwie-lubelskim", "Uwaga! W dniach 3-5.10 będzie zrzucana szczepionka dla lisów przeciw wściekliźnie. Nie dotykaj kapsułki i nie dopuszczaj do niej zwierząt domowych!", new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "https://www.gov.pl/photo/format/a5985200-c6d2-4418-97f7-7cc062694178/resolution/1920x810", "Alert RCB - szczepionka na lisy przeciw wściekliźnie w województwie lubelskim (3-5.10)" },
                    { 2, "https://www.gov.pl/web/rcb/alert-rcb--szczepionka-na-lisy-przeciw-wsciekliznie-w-wojewodztwie-podkarpackim-2-1110", "Uwaga! W dniach 2-11.10 będzie zrzucana szczepionka dla lisów przeciw wściekliźnie. Nie dotykaj kapsułki i nie dopuszczaj do niej zwierząt domowych!", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "https://www.gov.pl/photo/format/187bfdf1-20ce-4713-b0ca-9e306961951a/resolution/1920x810", "Alert RCB - szczepionka na lisy przeciw wściekliźnie w województwie podkarpackim (2-11.10)" },
                    { 3, "https://www.gov.pl/web/rcb/alert-rcb--woda-niezdatna-do-spozycia-z-wodociagu-dzieslaw-2909", "Uwaga! Woda niezdatna do spożycia z wodociągu sieciowego Dziesław. Śledź lokalne komunikaty.", new DateTime(2025, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "https://www.gov.pl/photo/format/6ad9aa2b-a92f-4edc-b459-3ff97fc9f35c/resolution/1920x810", "Alert RCB - woda niezdatna do spożycia z wodociągu Dziesław (29.09)" }
                });

            migrationBuilder.InsertData(
                table: "LocationsRcb",
                columns: new[] { "Id", "Name", "RcbId" },
                values: new object[,]
                {
                    { 1, "mazowieckie", 1 },
                    { 2, "podkarpacie", 2 },
                    { 3, "dzieslaw", 3 }
                });

            migrationBuilder.InsertData(
                table: "TagsRcb",
                columns: new[] { "Id", "Name", "RcbId" },
                values: new object[,]
                {
                    { 1, "szczepionka", 1 },
                    { 2, "zwierzęta", 1 },
                    { 3, "szczepionka", 2 },
                    { 4, "zwierzęta", 2 },
                    { 5, "woda", 3 },
                    { 6, "chroby", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationsRcb",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LocationsRcb",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LocationsRcb",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TagsRcb",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TagsRcb",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TagsRcb",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TagsRcb",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TagsRcb",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TagsRcb",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rcbs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rcbs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rcbs",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
