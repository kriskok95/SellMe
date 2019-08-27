namespace SellMe.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedDbSetPromotions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotion_Ads_AdId",
                table: "Promotion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promotion",
                table: "Promotion");

            migrationBuilder.RenameTable(
                name: "Promotion",
                newName: "Promotions");

            migrationBuilder.RenameIndex(
                name: "IX_Promotion_AdId",
                table: "Promotions",
                newName: "IX_Promotions_AdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promotions",
                table: "Promotions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Ads_AdId",
                table: "Promotions",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Ads_AdId",
                table: "Promotions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promotions",
                table: "Promotions");

            migrationBuilder.RenameTable(
                name: "Promotions",
                newName: "Promotion");

            migrationBuilder.RenameIndex(
                name: "IX_Promotions_AdId",
                table: "Promotion",
                newName: "IX_Promotion_AdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promotion",
                table: "Promotion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotion_Ads_AdId",
                table: "Promotion",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
