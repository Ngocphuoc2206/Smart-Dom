using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Dom.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRooomReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsImages_RoomReviews_RoomReviewId",
                table: "ReviewsImages");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomReviews_Users_UserID",
                table: "RoomReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomReviews",
                table: "RoomReviews");

            migrationBuilder.RenameTable(
                name: "RoomReviews",
                newName: "RoomReviewModel");

            migrationBuilder.RenameIndex(
                name: "IX_RoomReviews_UserID",
                table: "RoomReviewModel",
                newName: "IX_RoomReviewModel_UserID");

            migrationBuilder.AlterColumn<string>(
                name: "ResponseFromOwner",
                table: "RoomReviewModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResponseDate",
                table: "RoomReviewModel",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomReviewModel",
                table: "RoomReviewModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsImages_RoomReviewModel_RoomReviewId",
                table: "ReviewsImages",
                column: "RoomReviewId",
                principalTable: "RoomReviewModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomReviewModel_Users_UserID",
                table: "RoomReviewModel",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsImages_RoomReviewModel_RoomReviewId",
                table: "ReviewsImages");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomReviewModel_Users_UserID",
                table: "RoomReviewModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomReviewModel",
                table: "RoomReviewModel");

            migrationBuilder.RenameTable(
                name: "RoomReviewModel",
                newName: "RoomReviews");

            migrationBuilder.RenameIndex(
                name: "IX_RoomReviewModel_UserID",
                table: "RoomReviews",
                newName: "IX_RoomReviews_UserID");

            migrationBuilder.AlterColumn<string>(
                name: "ResponseFromOwner",
                table: "RoomReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResponseDate",
                table: "RoomReviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomReviews",
                table: "RoomReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsImages_RoomReviews_RoomReviewId",
                table: "ReviewsImages",
                column: "RoomReviewId",
                principalTable: "RoomReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomReviews_Users_UserID",
                table: "RoomReviews",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
