using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductLocationForecastsToProductLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ProductLocationForecasts",
                newName: "ProductLocations");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "ProductLocations",
                newName: "PlannedAmount");

            migrationBuilder.RenameIndex(
                name: "IX_ProductLocationForecasts_ProductId_LocationId_EventYear",
                table: "ProductLocations",
                newName: "IX_ProductLocations_ProductId_LocationId_EventYear");

            migrationBuilder.RenameIndex(
                name: "IX_ProductLocationForecasts_LocationId",
                table: "ProductLocations",
                newName: "IX_ProductLocations_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductLocationForecasts_EventYear",
                table: "ProductLocations",
                newName: "IX_ProductLocations_EventYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_ProductLocations_EventYear",
                table: "ProductLocations",
                newName: "IX_ProductLocationForecasts_EventYear");

            migrationBuilder.RenameIndex(
                name: "IX_ProductLocations_LocationId",
                table: "ProductLocations",
                newName: "IX_ProductLocationForecasts_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductLocations_ProductId_LocationId_EventYear",
                table: "ProductLocations",
                newName: "IX_ProductLocationForecasts_ProductId_LocationId_EventYear");

            migrationBuilder.RenameColumn(
                name: "PlannedAmount",
                table: "ProductLocations",
                newName: "Amount");

            migrationBuilder.RenameTable(
                name: "ProductLocations",
                newName: "ProductLocationForecasts");
        }
    }
}
