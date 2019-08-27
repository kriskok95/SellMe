namespace SellMe.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedPromotionOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Ads_AdId",
                table: "Promotions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_AdId",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "ActiveTo",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "AdId",
                table: "Promotions");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Promotions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "PromotionOrders",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    AdId = table.Column<int>(),
                    PromotionId = table.Column<int>(),
                    Quantity = table.Column<int>(),
                    Price = table.Column<decimal>(),
                    ActiveTo = table.Column<DateTime>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionOrders_Ads_AdId",
                        column: x => x.AdId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionOrders_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionOrders_AdId",
                table: "PromotionOrders",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionOrders_PromotionId",
                table: "PromotionOrders",
                column: "PromotionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Promotions");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActiveTo",
                table: "Promotions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AdId",
                table: "Promotions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdId = table.Column<int>(),
                    CreatedOn = table.Column<DateTime>(),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)"),
                    Quantity = table.Column<int>(),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Ads_AdId",
                        column: x => x.AdId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_AdId",
                table: "Promotions",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AdId",
                table: "Orders",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Ads_AdId",
                table: "Promotions",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
