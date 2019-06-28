using Microsoft.EntityFrameworkCore.Migrations;

namespace SellMe.Data.Migrations
{
    public partial class AddedDistrictToAddresAndChangedTypeOfZipCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ZipCode",
                table: "Addresses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Addresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
