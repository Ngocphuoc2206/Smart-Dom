using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Dom.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaintenanceREquest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MaintenanceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "MaintenanceRequests");
        }
    }
}
