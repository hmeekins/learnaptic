using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnaptic.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLastAccessed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastAccessed",
                table: "StudyGuides",
                newName: "LastAccessedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastAccessedAt",
                table: "StudyGuides",
                newName: "LastAccessed");
        }
    }
}
