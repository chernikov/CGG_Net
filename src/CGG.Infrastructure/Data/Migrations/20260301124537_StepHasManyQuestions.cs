using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class StepHasManyQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveySteps_SurveyQuestions_QuestionId",
                table: "SurveySteps");

            migrationBuilder.DropIndex(
                name: "IX_SurveySteps_QuestionId",
                table: "SurveySteps");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "SurveySteps");

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "SurveyQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "StepId",
                table: "SurveyQuestions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_StepId",
                table: "SurveyQuestions",
                column: "StepId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestions_SurveySteps_StepId",
                table: "SurveyQuestions",
                column: "StepId",
                principalTable: "SurveySteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestions_SurveySteps_StepId",
                table: "SurveyQuestions");

            migrationBuilder.DropIndex(
                name: "IX_SurveyQuestions_StepId",
                table: "SurveyQuestions");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "SurveyQuestions");

            migrationBuilder.DropColumn(
                name: "StepId",
                table: "SurveyQuestions");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "SurveySteps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SurveySteps_QuestionId",
                table: "SurveySteps",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveySteps_SurveyQuestions_QuestionId",
                table: "SurveySteps",
                column: "QuestionId",
                principalTable: "SurveyQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
