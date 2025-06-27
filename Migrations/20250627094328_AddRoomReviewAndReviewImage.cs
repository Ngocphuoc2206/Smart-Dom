using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Dom.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomReviewAndReviewImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverallRating = table.Column<int>(type: "int", nullable: false),
                    CleanlinessRating = table.Column<int>(type: "int", nullable: false),
                    AmenitiesRating = table.Column<int>(type: "int", nullable: false),
                    LocationRating = table.Column<int>(type: "int", nullable: false),
                    ValueForMoneyRating = table.Column<int>(type: "int", nullable: false),
                    HostAttitudeRating = table.Column<int>(type: "int", nullable: false),
                    ExperienceComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewsImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomReviewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewsImages_RoomReviews_RoomReviewId",
                        column: x => x.RoomReviewId,
                        principalTable: "RoomReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewsImages_RoomReviewId",
                table: "ReviewsImages",
                column: "RoomReviewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewsImages");

            migrationBuilder.DropTable(
                name: "RoomReviews");
        }
    }
}
