using Microsoft.EntityFrameworkCore.Migrations;

namespace SellMe.Data.Migrations
{
    public partial class UpdatedPromotionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Promotion_PromotionId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_PromotionId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Ads");

            migrationBuilder.AddColumn<int>(
                name: "AdId",
                table: "Promotion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_AdId",
                table: "Promotion",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotion_Ads_AdId",
                table: "Promotion",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotion_Ads_AdId",
                table: "Promotion");

            migrationBuilder.DropIndex(
                name: "IX_Promotion_AdId",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "AdId",
                table: "Promotion");

            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "Ads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_PromotionId",
                table: "Ads",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Promotion_PromotionId",
                table: "Ads",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
