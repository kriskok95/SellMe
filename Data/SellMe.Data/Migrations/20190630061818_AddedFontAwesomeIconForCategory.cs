using Microsoft.EntityFrameworkCore.Migrations;

namespace SellMe.Data.Migrations
{
    public partial class AddedFontAwesomeIconForCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FontAwesomeIcon",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FontAwesomeIcon",
                table: "Categories");
        }
    }
}
