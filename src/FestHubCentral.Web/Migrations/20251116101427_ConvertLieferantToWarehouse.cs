using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class ConvertLieferantToWarehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update existing InventoryTransfers to convert Lieferant (ID 28) references to null (warehouse)
            migrationBuilder.Sql(@"
                UPDATE ""InventoryTransfers""
                SET ""FromVendorId"" = NULL
                WHERE ""FromVendorId"" = 28;
            ");

            migrationBuilder.Sql(@"
                UPDATE ""InventoryTransfers""
                SET ""ToVendorId"" = NULL
                WHERE ""ToVendorId"" = 28;
            ");

            // Update existing Inventories to convert Lieferant (ID 28) references to null (warehouse)
            migrationBuilder.Sql(@"
                UPDATE ""Inventories""
                SET ""VendorId"" = NULL
                WHERE ""VendorId"" = 28;
            ");

            // Update vendor categories for OK-Ackerstein and OK-Kirche to Warehouse
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

            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 28,
                column: "Category",
                value: "Warehouse");

            // Delete the Lieferant vendor record (ID 28) now that all references have been updated
            migrationBuilder.DeleteData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 28);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 28,
                column: "Category",
                value: "Mixed");
        }
    }
}
