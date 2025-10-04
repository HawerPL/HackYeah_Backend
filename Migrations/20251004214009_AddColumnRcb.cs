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
                name: "ArticleUrl",
                table: "Rcbs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "QuestionAnswers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleUrl",
                table: "Rcbs");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "QuestionAnswers");
        }
    }
}
