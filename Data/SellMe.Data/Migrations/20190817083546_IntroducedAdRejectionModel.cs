namespace SellMe.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class IntroducedAdRejectionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdRejections",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    AdId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdRejections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdRejections_Ads_AdId",
                        column: x => x.AdId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdRejections_AdId",
                table: "AdRejections",
                column: "AdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdRejections");
        }
    }
}
