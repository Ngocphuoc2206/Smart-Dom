using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Dom.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Rooms_IDRoom",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Users_IDUser",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_IDRoom",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IDRoom",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "IDUser",
                table: "Invoices",
                newName: "ContractID");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_IDUser",
                table: "Invoices",
                newName: "IX_Invoices_ContractID");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Contracts_ContractID",
                table: "Invoices",
                column: "ContractID",
                principalTable: "Contracts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Contracts_ContractID",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "ContractID",
                table: "Invoices",
                newName: "IDUser");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ContractID",
                table: "Invoices",
                newName: "IX_Invoices_IDUser");

            migrationBuilder.AddColumn<int>(
                name: "IDRoom",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_IDRoom",
                table: "Invoices",
                column: "IDRoom");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Rooms_IDRoom",
                table: "Invoices",
                column: "IDRoom",
                principalTable: "Rooms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Users_IDUser",
                table: "Invoices",
                column: "IDUser",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
