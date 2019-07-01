using Microsoft.EntityFrameworkCore.Migrations;

namespace SellMe.Data.Migrations
{
    public partial class FixedSubcategoryRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_SubCategories_CategoryId",
                table: "Ads");

            migrationBuilder.CreateIndex(
                name: "IX_Ads_SubCategoryId",
                table: "Ads",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_SubCategories_SubCategoryId",
                table: "Ads",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_SubCategories_SubCategoryId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_SubCategoryId",
                table: "Ads");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_SubCategories_CategoryId",
                table: "Ads",
                column: "CategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
