using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class FinalizeWarehouseChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 26,
                column: "Category",
                value: "Warehouse");

            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 27,
                column: "Category",
                value: "Warehouse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 26,
                column: "Category",
                value: "Mixed");

            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 27,
                column: "Category",
                value: "Mixed");

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Id", "Category", "ContactEmail", "ContactPerson", "ContactPhone", "CreatedAt", "LocationSpot", "Name", "UpdatedAt" },
                values: new object[] { 28, "Warehouse", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, "Lieferant", null });
        }
    }
}
