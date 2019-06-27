using Microsoft.EntityFrameworkCore.Migrations;

namespace SellMe.Data.Migrations
{
    public partial class AddedSellerForAd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SellerId",
                table: "Ads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_SellerId",
                table: "Ads",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_AspNetUsers_SellerId",
                table: "Ads",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_AspNetUsers_SellerId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_SellerId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Ads");
        }
    }
}
