using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameToEnglishPriceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Verkaufspreis",
                table: "ProductEventPrices",
                newName: "SellingPrice");

            migrationBuilder.RenameColumn(
                name: "Einkaufspreis",
                table: "ProductEventPrices",
                newName: "PurchasePrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "ProductEventPrices",
                newName: "Verkaufspreis");

            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "ProductEventPrices",
                newName: "Einkaufspreis");
        }
    }
}
