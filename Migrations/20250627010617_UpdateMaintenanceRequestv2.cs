using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Dom.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaintenanceRequestv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponeFromOwners",
                table: "MaintenanceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponeFromOwners",
                table: "MaintenanceRequests");
        }
    }
}
