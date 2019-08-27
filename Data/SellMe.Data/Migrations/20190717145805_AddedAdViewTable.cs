namespace SellMe.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedAdViewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdViews",
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
                    table.PrimaryKey("PK_AdViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdViews_Ads_AdId",
                        column: x => x.AdId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdViews_AdId",
                table: "AdViews",
                column: "AdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdViews");
        }
    }
}
