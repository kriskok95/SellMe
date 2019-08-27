namespace SellMe.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemovedPrieFromPromotionOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PromotionOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PromotionOrders",
                nullable: false,
                defaultValue: 0);
        }
    }
}
