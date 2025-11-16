using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameVendorToLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Vendors_VendorId",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_CashRegisters_Vendors_VendorId",
                table: "CashRegisters");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Vendors_VendorId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransfers_Vendors_FromVendorId",
                table: "InventoryTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransfers_Vendors_ToVendorId",
                table: "InventoryTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Vendors_VendorId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "Orders",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_VendorId",
                table: "Orders",
                newName: "IX_Orders_LocationId");

            migrationBuilder.RenameColumn(
                name: "ToVendorId",
                table: "InventoryTransfers",
                newName: "ToLocationId");

            migrationBuilder.RenameColumn(
                name: "FromVendorId",
                table: "InventoryTransfers",
                newName: "FromLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransfers_ToVendorId",
                table: "InventoryTransfers",
                newName: "IX_InventoryTransfers_ToLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransfers_FromVendorId",
                table: "InventoryTransfers",
                newName: "IX_InventoryTransfers_FromLocationId");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "Inventories",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_VendorId",
                table: "Inventories",
                newName: "IX_Inventories_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_ProductId_VendorId_EventYear",
                table: "Inventories",
                newName: "IX_Inventories_ProductId_LocationId_EventYear");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "CashRegisters",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_CashRegisters_VendorId",
                table: "CashRegisters",
                newName: "IX_CashRegisters_LocationId");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "Alerts",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Alerts_VendorId",
                table: "Alerts",
                newName: "IX_Alerts_LocationId");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LocationSpot = table.Column<int>(type: "integer", nullable: false),
                    ContactPerson = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ContactEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Category", "ContactEmail", "ContactPerson", "ContactPhone", "CreatedAt", "LocationSpot", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 15, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Wiilaube", null },
                    { 16, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Raclett-Zelt", null },
                    { 17, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Winzerlounge", null },
                    { 18, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Wümmetkafi", null },
                    { 19, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, "Wurst/Getränke Kirche", null },
                    { 20, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "Wurst/Getränke Ackersteinstrasse", null },
                    { 21, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, "Bar Ackersteinstrasse", null },
                    { 22, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, "Bar Mühlehalde", null },
                    { 23, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "Kiwanis", null },
                    { 24, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, "Rebhüsli", null },
                    { 25, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Fischstand", null },
                    { 26, "Staging Area", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, "OK-Ackerstein", null },
                    { 27, "Staging Area", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, "OK-Kirche", null },
                    { 29, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15, "Crêpes Stand", null },
                    { 30, "Vendor", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "Suuserwagen", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationSpot",
                table: "Locations",
                column: "LocationSpot",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Locations_LocationId",
                table: "Alerts",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CashRegisters_Locations_LocationId",
                table: "CashRegisters",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Locations_LocationId",
                table: "Inventories",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransfers_Locations_FromLocationId",
                table: "InventoryTransfers",
                column: "FromLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransfers_Locations_ToLocationId",
                table: "InventoryTransfers",
                column: "ToLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Locations_LocationId",
                table: "Orders",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Locations_LocationId",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_CashRegisters_Locations_LocationId",
                table: "CashRegisters");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Locations_LocationId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransfers_Locations_FromLocationId",
                table: "InventoryTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransfers_Locations_ToLocationId",
                table: "InventoryTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Locations_LocationId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Orders",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_LocationId",
                table: "Orders",
                newName: "IX_Orders_VendorId");

            migrationBuilder.RenameColumn(
                name: "ToLocationId",
                table: "InventoryTransfers",
                newName: "ToVendorId");

            migrationBuilder.RenameColumn(
                name: "FromLocationId",
                table: "InventoryTransfers",
                newName: "FromVendorId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransfers_ToLocationId",
                table: "InventoryTransfers",
                newName: "IX_InventoryTransfers_ToVendorId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransfers_FromLocationId",
                table: "InventoryTransfers",
                newName: "IX_InventoryTransfers_FromVendorId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Inventories",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_ProductId_LocationId_EventYear",
                table: "Inventories",
                newName: "IX_Inventories_ProductId_VendorId_EventYear");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_LocationId",
                table: "Inventories",
                newName: "IX_Inventories_VendorId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "CashRegisters",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_CashRegisters_LocationId",
                table: "CashRegisters",
                newName: "IX_CashRegisters_VendorId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Alerts",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Alerts_LocationId",
                table: "Alerts",
                newName: "IX_Alerts_VendorId");

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ContactEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ContactPerson = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LocationSpot = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Id", "Category", "ContactEmail", "ContactPerson", "ContactPhone", "CreatedAt", "LocationSpot", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 15, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Wiilaube", null },
                    { 16, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Raclett-Zelt", null },
                    { 17, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Winzerlounge", null },
                    { 18, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Wümmetkafi", null },
                    { 19, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, "Wurst/Getränke Kirche", null },
                    { 20, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, "Wurst/Getränke Ackersteinstrasse", null },
                    { 21, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, "Bar Ackersteinstrasse", null },
                    { 22, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, "Bar Mühlehalde", null },
                    { 23, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "Kiwanis", null },
                    { 24, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, "Rebhüsli", null },
                    { 25, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Fischstand", null },
                    { 26, "Warehouse", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, "OK-Ackerstein", null },
                    { 27, "Warehouse", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, "OK-Kirche", null },
                    { 29, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15, "Crêpes Stand", null },
                    { 30, "Mixed", null, null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, "Suuserwagen", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_LocationSpot",
                table: "Vendors",
                column: "LocationSpot",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Vendors_VendorId",
                table: "Alerts",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CashRegisters_Vendors_VendorId",
                table: "CashRegisters",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Vendors_VendorId",
                table: "Inventories",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransfers_Vendors_FromVendorId",
                table: "InventoryTransfers",
                column: "FromVendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransfers_Vendors_ToVendorId",
                table: "InventoryTransfers",
                column: "ToVendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Vendors_VendorId",
                table: "Orders",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
