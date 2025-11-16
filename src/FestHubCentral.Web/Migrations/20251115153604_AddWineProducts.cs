using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddWineProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedAt", "Description", "IsAvailable", "Name", "SupplierId", "Unit", "UpdatedAt" },
                values: new object[,]
                {
                    { 122, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Festwein 50 cl - 15er Harass", true, "Weiss Riesling-Silvaner Classique AOC 50cl", 11, "Bottle", null },
                    { 123, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Festwein 50 cl - 15er Harass", true, "Rosé Classique AOC 50cl", 11, "Bottle", null },
                    { 124, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Festwein 50 cl - 15er Harass", true, "Rotwein Pinot Noir Zürich AOC 50 cl", 12, "Bottle", null },
                    { 125, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Weisswein 75 cl - 6er Karton", true, "Riesling-Silvaner vom Chillesteig AOC 2023", 13, "Bottle", null },
                    { 126, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Weisswein 75 cl - 12er Karton", true, "Riesling-Silvaner Classique AOC 2023", 11, "Bottle", null },
                    { 127, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Weisswein 75 cl - 6er Karton", true, "Riesling-Silvaner Stadt Züri, Lässig AOC 2023", 12, "Bottle", null },
                    { 128, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Weisswein 75 cl - 6er Karton", true, "Pinot Gris vom Chillesteig AOC 2023", 13, "Bottle", null },
                    { 129, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Weisswein 75 cl - 6er Karton", true, "Le Gris Spécial AOC 2023", 11, "Bottle", null },
                    { 130, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Weisswein 75 cl - 6er Karton", true, "Räuschling vom Chillesteig AOC 2023", 13, "Bottle", null },
                    { 131, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Weisswein 75 cl - 6er Karton", true, "Loyal, Räuschling AOC 2023", 12, "Bottle", null }
                });

            migrationBuilder.InsertData(
                table: "ProductEventPrices",
                columns: new[] { "Id", "CreatedAt", "EventYear", "Price", "ProductId", "UpdatedAt" },
                values: new object[,]
                {
                    { 17, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, 21.00m, 122, null },
                    { 18, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, 21.00m, 123, null },
                    { 19, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, 21.00m, 124, null },
                    { 20, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, 40.00m, 125, null },
                    { 21, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, 40.00m, 126, null },
                    { 22, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, 40.00m, 127, null },
                    { 23, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, 40.00m, 128, null },
                    { 24, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, 45.00m, 129, null },
                    { 25, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, 45.00m, 130, null },
                    { 26, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024, 45.00m, 131, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductEventPrices",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 131);
        }
    }
}
