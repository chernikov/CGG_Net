using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamePurposeCategoryToPurpose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurposeCategory",
                table: "SurveyQuestions",
                newName: "Purpose");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyQuestions_PurposeCategory",
                table: "SurveyQuestions",
                newName: "IX_SurveyQuestions_Purpose");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Purpose",
                table: "SurveyQuestions",
                newName: "PurposeCategory");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyQuestions_Purpose",
                table: "SurveyQuestions",
                newName: "IX_SurveyQuestions_PurposeCategory");
        }
    }
}
