namespace SellMe.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedActiveDaysColumnAtPromotionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveDays",
                table: "Promotions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveDays",
                table: "Promotions");
        }
    }
}
