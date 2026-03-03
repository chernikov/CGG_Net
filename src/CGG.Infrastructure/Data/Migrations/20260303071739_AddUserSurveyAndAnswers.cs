using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSurveyAndAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSurveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SurveyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSurveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSurveys_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserSurveys_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSurveyAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserSurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSurveyAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSurveyAnswers_UserSurveys_UserSurveyId",
                        column: x => x.UserSurveyId,
                        principalTable: "UserSurveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSurveyAnswers_Survey_Step_Question",
                table: "UserSurveyAnswers",
                columns: new[] { "UserSurveyId", "StepNumber", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSurveys_StartedAt",
                table: "UserSurveys",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserSurveys_Status",
                table: "UserSurveys",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UserSurveys_SurveyId",
                table: "UserSurveys",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSurveys_UserId_SurveyType",
                table: "UserSurveys",
                columns: new[] { "UserId", "SurveyType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSurveyAnswers");

            migrationBuilder.DropTable(
                name: "UserSurveys");
        }
    }
}
