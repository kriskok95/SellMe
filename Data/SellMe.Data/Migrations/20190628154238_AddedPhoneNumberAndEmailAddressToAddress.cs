using Microsoft.EntityFrameworkCore.Migrations;

namespace SellMe.Data.Migrations
{
    public partial class AddedPhoneNumberAndEmailAddressToAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Addresses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber",
                table: "Addresses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Addresses");
        }
    }
}
