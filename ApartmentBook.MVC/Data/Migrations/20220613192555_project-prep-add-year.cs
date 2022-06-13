using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentBook.MVC.Data.Migrations
{
    public partial class projectprepaddyear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentYear",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentYear",
                table: "Payments");
        }
    }
}
