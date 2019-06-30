using Microsoft.EntityFrameworkCore.Migrations;

namespace SellMe.Data.Migrations
{
    public partial class RemovedNullableAddressIdForAd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Ads",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Ads",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
