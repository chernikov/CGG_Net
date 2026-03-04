using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTagsAndCreatedByFromAiPromptTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiPromptTemplates_Users_CreatedByUserId",
                table: "AiPromptTemplates");

            migrationBuilder.DropIndex(
                name: "IX_AiPromptTemplates_CreatedByUserId",
                table: "AiPromptTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "AiPromptTemplates");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "AiPromptTemplates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "AiPromptTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "AiPromptTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AiPromptTemplates_CreatedByUserId",
                table: "AiPromptTemplates",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AiPromptTemplates_Users_CreatedByUserId",
                table: "AiPromptTemplates",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
