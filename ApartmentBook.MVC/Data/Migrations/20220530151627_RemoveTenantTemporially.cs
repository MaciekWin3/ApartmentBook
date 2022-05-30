using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentBook.MVC.Data.Migrations
{
    public partial class RemoveTenantTemporially : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Tenant_TenantId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "Tenant");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Payments",
                newName: "ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_TenantId",
                table: "Payments",
                newName: "IX_Payments_ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Apartments_ApartmentId",
                table: "Payments",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Apartments_ApartmentId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "ApartmentId",
                table: "Payments",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ApartmentId",
                table: "Payments",
                newName: "IX_Payments_TenantId");

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenant_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_ApartmentId",
                table: "Tenant",
                column: "ApartmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Tenant_TenantId",
                table: "Payments",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id");
        }
    }
}
