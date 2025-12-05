using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInventoryStockFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Locations_LocationId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_LocationId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_ProductId_LocationId_EventYear",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CurrentStock",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "LastRestocked",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "MaximumStock",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "ReorderQuantity",
                table: "Inventories");

            migrationBuilder.Sql(@"
                DELETE FROM ""Inventories"" a USING ""Inventories"" b
                WHERE a.""Id"" < b.""Id""
                AND a.""ProductId"" = b.""ProductId""
                AND a.""EventYear"" = b.""EventYear"";
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId_EventYear",
                table: "Inventories",
                columns: new[] { "ProductId", "EventYear" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Inventories_ProductId_EventYear",
                table: "Inventories");

            migrationBuilder.AddColumn<int>(
                name: "CurrentStock",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastRestocked",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Inventories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumStock",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumStock",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReorderQuantity",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_LocationId",
                table: "Inventories",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId_LocationId_EventYear",
                table: "Inventories",
                columns: new[] { "ProductId", "LocationId", "EventYear" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Locations_LocationId",
                table: "Inventories",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
