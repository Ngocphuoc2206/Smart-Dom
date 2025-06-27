using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Dom.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomReviewv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "RoomReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RoomReviews_UserID",
                table: "RoomReviews",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomReviews_Users_UserID",
                table: "RoomReviews",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomReviews_Users_UserID",
                table: "RoomReviews");

            migrationBuilder.DropIndex(
                name: "IX_RoomReviews_UserID",
                table: "RoomReviews");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "RoomReviews");
        }
    }
}
