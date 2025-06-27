using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Dom.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomReviewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponseFromOwner",
                table: "RoomReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseFromOwner",
                table: "RoomReviews");
        }
    }
}
