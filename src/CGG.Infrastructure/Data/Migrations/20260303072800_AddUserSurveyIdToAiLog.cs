using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSurveyIdToAiLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserSurveyId",
                table: "AiLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_UserSurveyId",
                table: "AiLogs",
                column: "UserSurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AiLogs_UserSurveys_UserSurveyId",
                table: "AiLogs",
                column: "UserSurveyId",
                principalTable: "UserSurveys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiLogs_UserSurveys_UserSurveyId",
                table: "AiLogs");

            migrationBuilder.DropIndex(
                name: "IX_AiLogs_UserSurveyId",
                table: "AiLogs");

            migrationBuilder.DropColumn(
                name: "UserSurveyId",
                table: "AiLogs");
        }
    }
}
