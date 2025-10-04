using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackYeah_Backend.Migrations
{
    /// <inheritdoc />
    public partial class NewTablesAndDropColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Rcbs");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Rcbs");

            migrationBuilder.CreateTable(
                name: "LocationsRcb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RcbId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsRcb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationsRcb_Rcbs_RcbId",
                        column: x => x.RcbId,
                        principalTable: "Rcbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagsRcb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RcbId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagsRcb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagsRcb_Rcbs_RcbId",
                        column: x => x.RcbId,
                        principalTable: "Rcbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationsRcb_RcbId",
                table: "LocationsRcb",
                column: "RcbId");

            migrationBuilder.CreateIndex(
                name: "IX_TagsRcb_RcbId",
                table: "TagsRcb",
                column: "RcbId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationsRcb");

            migrationBuilder.DropTable(
                name: "TagsRcb");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Rcbs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Rcbs",
                type: "TEXT",
                nullable: true);
        }
    }
}
