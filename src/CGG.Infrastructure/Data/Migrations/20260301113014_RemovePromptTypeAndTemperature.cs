using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePromptTypeAndTemperature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AiPromptTemplates_PromptType",
                table: "AiPromptTemplates");

            migrationBuilder.DropIndex(
                name: "IX_AiPromptTemplates_PromptType_Version",
                table: "AiPromptTemplates");

            migrationBuilder.DropColumn(
                name: "PromptType",
                table: "AiPromptTemplates");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "AiPromptTemplates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PromptType",
                table: "AiPromptTemplates",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Temperature",
                table: "AiPromptTemplates",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AiPromptTemplates_PromptType",
                table: "AiPromptTemplates",
                column: "PromptType");

            migrationBuilder.CreateIndex(
                name: "IX_AiPromptTemplates_PromptType_Version",
                table: "AiPromptTemplates",
                columns: new[] { "PromptType", "Version" });
        }
    }
}
