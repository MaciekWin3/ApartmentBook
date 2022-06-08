using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentBook.MVC.Data.Migrations
{
    public partial class Updatemodels2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Meterage",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "Apartments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meterage",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Apartments");
        }
    }
}
