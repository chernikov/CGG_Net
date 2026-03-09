using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMemberSurveyResultFromAiLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiLogs_Members_MemberId",
                table: "AiLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AiLogs_SurveyResults_SurveyResultId",
                table: "AiLogs");

            migrationBuilder.DropIndex(
                name: "IX_AiLogs_MemberId",
                table: "AiLogs");

            migrationBuilder.DropIndex(
                name: "IX_AiLogs_MemberId_CreatedAt",
                table: "AiLogs");

            migrationBuilder.DropIndex(
                name: "IX_AiLogs_SurveyResultId",
                table: "AiLogs");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "AiLogs");

            migrationBuilder.DropColumn(
                name: "SurveyResultId",
                table: "AiLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MemberId",
                table: "AiLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SurveyResultId",
                table: "AiLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_MemberId",
                table: "AiLogs",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_MemberId_CreatedAt",
                table: "AiLogs",
                columns: new[] { "MemberId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_SurveyResultId",
                table: "AiLogs",
                column: "SurveyResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_AiLogs_Members_MemberId",
                table: "AiLogs",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AiLogs_SurveyResults_SurveyResultId",
                table: "AiLogs",
                column: "SurveyResultId",
                principalTable: "SurveyResults",
                principalColumn: "Id");
        }
    }
}
