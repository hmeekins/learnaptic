using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnaptic.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "StudySets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "StudyGuides",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudySets_UserId",
                table: "StudySets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGuides_UserId",
                table: "StudyGuides",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGuides_AspNetUsers_UserId",
                table: "StudyGuides",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudySets_AspNetUsers_UserId",
                table: "StudySets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGuides_AspNetUsers_UserId",
                table: "StudyGuides");

            migrationBuilder.DropForeignKey(
                name: "FK_StudySets_AspNetUsers_UserId",
                table: "StudySets");

            migrationBuilder.DropIndex(
                name: "IX_StudySets_UserId",
                table: "StudySets");

            migrationBuilder.DropIndex(
                name: "IX_StudyGuides_UserId",
                table: "StudyGuides");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StudySets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StudyGuides");
        }
    }
}
