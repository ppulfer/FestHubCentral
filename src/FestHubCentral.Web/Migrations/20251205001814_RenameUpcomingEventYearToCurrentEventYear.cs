using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FestHubCentral.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameUpcomingEventYearToCurrentEventYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_Events_UpcomingEventYear",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "UpcomingEventYear",
                table: "Settings",
                newName: "CurrentEventYear");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_UpcomingEventYear",
                table: "Settings",
                newName: "IX_Settings_CurrentEventYear");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_Events_CurrentEventYear",
                table: "Settings",
                column: "CurrentEventYear",
                principalTable: "Events",
                principalColumn: "Year",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_Events_CurrentEventYear",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "CurrentEventYear",
                table: "Settings",
                newName: "UpcomingEventYear");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_CurrentEventYear",
                table: "Settings",
                newName: "IX_Settings_UpcomingEventYear");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_Events_UpcomingEventYear",
                table: "Settings",
                column: "UpcomingEventYear",
                principalTable: "Events",
                principalColumn: "Year",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
