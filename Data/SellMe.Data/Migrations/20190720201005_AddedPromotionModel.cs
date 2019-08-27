namespace SellMe.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedPromotionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "Ads",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ActiveTo = table.Column<DateTime>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Promotion_PromotionId",
                table: "Ads");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropIndex(
                name: "IX_Ads_PromotionId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Ads");
        }
    }
}
