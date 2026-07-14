using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learnaptic.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStudySets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_StudyGuides_StudyGuideId",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "StudyGuideId",
                table: "Flashcards",
                newName: "StudySetId");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_StudyGuideId",
                table: "Flashcards",
                newName: "IX_Flashcards_StudySetId");

            migrationBuilder.CreateTable(
                name: "StudySets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastAccessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudySets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudyGuideStudySet",
                columns: table => new
                {
                    StudyGuidesId = table.Column<int>(type: "integer", nullable: false),
                    StudySetsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyGuideStudySet", x => new { x.StudyGuidesId, x.StudySetsId });
                    table.ForeignKey(
                        name: "FK_StudyGuideStudySet_StudyGuides_StudyGuidesId",
                        column: x => x.StudyGuidesId,
                        principalTable: "StudyGuides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyGuideStudySet_StudySets_StudySetsId",
                        column: x => x.StudySetsId,
                        principalTable: "StudySets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyGuideStudySet_StudySetsId",
                table: "StudyGuideStudySet",
                column: "StudySetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_StudySets_StudySetId",
                table: "Flashcards",
                column: "StudySetId",
                principalTable: "StudySets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_StudySets_StudySetId",
                table: "Flashcards");

            migrationBuilder.DropTable(
                name: "StudyGuideStudySet");

            migrationBuilder.DropTable(
                name: "StudySets");

            migrationBuilder.RenameColumn(
                name: "StudySetId",
                table: "Flashcards",
                newName: "StudyGuideId");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_StudySetId",
                table: "Flashcards",
                newName: "IX_Flashcards_StudyGuideId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_StudyGuides_StudyGuideId",
                table: "Flashcards",
                column: "StudyGuideId",
                principalTable: "StudyGuides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
