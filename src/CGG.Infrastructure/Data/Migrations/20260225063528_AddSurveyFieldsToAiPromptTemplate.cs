using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSurveyFieldsToAiPromptTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutputFormat",
                table: "AiPromptTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StepNumber",
                table: "AiPromptTemplates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurveyType",
                table: "AiPromptTemplates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutputFormat",
                table: "AiPromptTemplates");

            migrationBuilder.DropColumn(
                name: "StepNumber",
                table: "AiPromptTemplates");

            migrationBuilder.DropColumn(
                name: "SurveyType",
                table: "AiPromptTemplates");
        }
    }
}
