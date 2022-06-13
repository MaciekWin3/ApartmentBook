using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentBook.MVC.Data.Migrations
{
    public partial class projectprep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMonth",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantEmail",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMonth",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "TenantEmail",
                table: "Apartments");
        }
    }
}
