using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Dom.Migrations
{
    /// <inheritdoc />
    public partial class AddDurationContracts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationContractID",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DurationContracts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DurationContracts", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_DurationContractID",
                table: "Contracts",
                column: "DurationContractID");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_DurationContracts_DurationContractID",
                table: "Contracts",
                column: "DurationContractID",
                principalTable: "DurationContracts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_DurationContracts_DurationContractID",
                table: "Contracts");

            migrationBuilder.DropTable(
                name: "DurationContracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_DurationContractID",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "DurationContractID",
                table: "Contracts");
        }
    }
}
