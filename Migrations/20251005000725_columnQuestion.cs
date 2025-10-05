using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackYeah_Backend.Migrations
{
    /// <inheritdoc />
    public partial class columnQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Form",
                table: "Question",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "Id", "Description", "Form", "Order", "Title" },
                values: new object[] { 1, "Jaką masz płeć?", 0, 1, "płeć" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Form",
                table: "Question");
        }
    }
}
