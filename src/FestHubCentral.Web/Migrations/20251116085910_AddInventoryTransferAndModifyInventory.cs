using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryTransferAndModifyInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories");

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "Inventories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InventoryTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    FromVendorId = table.Column<int>(type: "integer", nullable: true),
                    ToVendorId = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EventYear = table.Column<int>(type: "integer", nullable: false),
                    TransferDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_Events_EventYear",
                        column: x => x.EventYear,
                        principalTable: "Events",
                        principalColumn: "Year",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_Vendors_FromVendorId",
                        column: x => x.FromVendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_Vendors_ToVendorId",
                        column: x => x.ToVendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId_VendorId_EventYear",
                table: "Inventories",
                columns: new[] { "ProductId", "VendorId", "EventYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_VendorId",
                table: "Inventories",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_CreatedByUserId",
                table: "InventoryTransfers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_EventYear_TransferDate",
                table: "InventoryTransfers",
                columns: new[] { "EventYear", "TransferDate" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_FromVendorId",
                table: "InventoryTransfers",
                column: "FromVendorId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_ProductId",
                table: "InventoryTransfers",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_ToVendorId",
                table: "InventoryTransfers",
                column: "ToVendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Vendors_VendorId",
                table: "Inventories",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Vendors_VendorId",
                table: "Inventories");

            migrationBuilder.DropTable(
                name: "InventoryTransfers");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_ProductId_VendorId_EventYear",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_VendorId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Inventories");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories",
                column: "ProductId",
                unique: true);
        }
    }
}
