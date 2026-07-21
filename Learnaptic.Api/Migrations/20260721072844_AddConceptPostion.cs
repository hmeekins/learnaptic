using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnaptic.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddConceptPostion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Concepts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Concepts");
        }
    }
}
