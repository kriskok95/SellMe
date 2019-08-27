namespace SellMe.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedIsApprovedColumnAtAdModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Ads",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Ads");
        }
    }
}
