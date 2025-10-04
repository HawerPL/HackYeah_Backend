using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackYeah_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnRcb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageRegionUrl",
                table: "Rcbs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageRegionUrl",
                table: "Rcbs");
        }
    }
}
