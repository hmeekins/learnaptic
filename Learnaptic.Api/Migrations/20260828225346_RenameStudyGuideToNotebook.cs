using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Learnaptic.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameStudyGuideToNotebook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concepts_StudyGuides_StudyGuideId",
                table: "Concepts");

            migrationBuilder.DropTable(
                name: "StudyGuideStudySet");

            migrationBuilder.DropTable(
                name: "StudyGuides");

            migrationBuilder.RenameColumn(
                name: "StudyGuideId",
                table: "Concepts",
                newName: "NotebookId");

            migrationBuilder.RenameIndex(
                name: "IX_Concepts_StudyGuideId",
                table: "Concepts",
                newName: "IX_Concepts_NotebookId");

            migrationBuilder.CreateTable(
                name: "Notebooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastAccessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notebooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notebooks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotebookStudySet",
                columns: table => new
                {
                    NotebooksId = table.Column<int>(type: "integer", nullable: false),
                    StudySetsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotebookStudySet", x => new { x.NotebooksId, x.StudySetsId });
                    table.ForeignKey(
                        name: "FK_NotebookStudySet_Notebooks_NotebooksId",
                        column: x => x.NotebooksId,
                        principalTable: "Notebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotebookStudySet_StudySets_StudySetsId",
                        column: x => x.StudySetsId,
                        principalTable: "StudySets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notebooks_UserId",
                table: "Notebooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotebookStudySet_StudySetsId",
                table: "NotebookStudySet",
                column: "StudySetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Concepts_Notebooks_NotebookId",
                table: "Concepts",
                column: "NotebookId",
                principalTable: "Notebooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concepts_Notebooks_NotebookId",
                table: "Concepts");

            migrationBuilder.DropTable(
                name: "NotebookStudySet");

            migrationBuilder.DropTable(
                name: "Notebooks");

            migrationBuilder.RenameColumn(
                name: "NotebookId",
                table: "Concepts",
                newName: "StudyGuideId");

            migrationBuilder.RenameIndex(
                name: "IX_Concepts_NotebookId",
                table: "Concepts",
                newName: "IX_Concepts_StudyGuideId");

            migrationBuilder.CreateTable(
                name: "StudyGuides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastAccessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyGuides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyGuides_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_StudyGuides_UserId",
                table: "StudyGuides",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGuideStudySet_StudySetsId",
                table: "StudyGuideStudySet",
                column: "StudySetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Concepts_StudyGuides_StudyGuideId",
                table: "Concepts",
                column: "StudyGuideId",
                principalTable: "StudyGuides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
