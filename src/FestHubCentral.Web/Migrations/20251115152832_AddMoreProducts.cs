using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedAt", "Description", "IsAvailable", "Name", "SupplierId", "Unit", "UpdatedAt" },
                values: new object[,]
                {
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Bratwurst", 1, "Piece", null },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Soft Drink", 2, "Bottle", null },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Wine (Red)", 2, "Glass", null },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Cheese Fondue", 3, "Portion", null }
                });

            migrationBuilder.UpdateData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EventYear", "Price", "ProductId" },
                values: new object[] { 2024, 6.50m, 5 });

            migrationBuilder.UpdateData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EventYear", "Price", "ProductId" },
                values: new object[] { 2024, 3.00m, 6 });

            migrationBuilder.UpdateData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EventYear", "Price", "ProductId" },
                values: new object[] { 2024, 7.00m, 7 });

            migrationBuilder.UpdateData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EventYear", "Price", "ProductId" },
                values: new object[] { 2024, 15.00m, 8 });

            migrationBuilder.InsertData(
                table: "ProductEventPrices",
                columns: new[] { "Id", "CreatedAt", "EventYear", "Price", "ProductId", "UpdatedAt" },
                values: new object[,]
                {
                    { 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, 8.50m, 1, null },
                    { 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, 4.00m, 2, null },
                    { 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, 6.00m, 3, null },
                    { 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, 12.00m, 4, null }
                });

            migrationBuilder.InsertData(
                table: "Inventories",
                columns: new[] { "Id", "CreatedAt", "CurrentStock", "EventYear", "LastRestocked", "MaximumStock", "MinimumStock", "ProductId", "ReorderQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 100, 2026, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 200, 25, 5, 100, null },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 120, 2026, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 250, 30, 6, 120, null },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 40, 2026, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 100, 10, 7, 50, null },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20, 2026, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 40, 5, 8, 20, null }
                });

            migrationBuilder.InsertData(
                table: "ProductEventPrices",
                columns: new[] { "Id", "CreatedAt", "EventYear", "Price", "ProductId", "UpdatedAt" },
                values: new object[,]
                {
                    { 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, 7.00m, 5, null },
                    { 14, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, 3.50m, 6, null },
                    { 15, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, 7.50m, 7, null },
                    { 16, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2026, 16.00m, 8, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EventYear", "Price", "ProductId" },
                values: new object[] { 2026, 8.50m, 1 });

            migrationBuilder.UpdateData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EventYear", "Price", "ProductId" },
                values: new object[] { 2026, 4.00m, 2 });

            migrationBuilder.UpdateData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EventYear", "Price", "ProductId" },
                values: new object[] { 2026, 6.00m, 3 });

            migrationBuilder.UpdateData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EventYear", "Price", "ProductId" },
                values: new object[] { 2026, 12.00m, 4 });
        }
    }
}
