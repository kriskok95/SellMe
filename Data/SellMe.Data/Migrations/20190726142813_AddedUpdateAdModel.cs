namespace SellMe.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedUpdateAdModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpdateAds",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    AdId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateAds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpdateAds_Ads_AdId",
                        column: x => x.AdId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpdateAds_AdId",
                table: "UpdateAds",
                column: "AdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateAds");
        }
    }
}
