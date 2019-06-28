using Microsoft.EntityFrameworkCore.Migrations;

namespace SellMe.Data.Migrations
{
    public partial class ChangedTheTypeOfPhoneNumberToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "Addresses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
