using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrandingSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FestivalName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LogoPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PrimaryColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SecondaryColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AccentColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tagline = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BrandingSettings",
                columns: new[] { "Id", "AccentColor", "FestivalName", "LogoPath", "PrimaryColor", "SecondaryColor", "Tagline", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, "#ec4899", "FestHub Central", null, "#6366f1", "#8b5cf6", "Real-time festival gastronomy management", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandingSettings");
        }
    }
}
