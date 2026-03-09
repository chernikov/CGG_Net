using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtractSurveyExampleAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswersJson",
                table: "SurveyExampleProfiles");

            migrationBuilder.CreateTable(
                name: "SurveyExampleAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ValueJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyExampleAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyExampleAnswers_SurveyExampleProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "SurveyExampleProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyExampleAnswers_ProfileId_Purpose",
                table: "SurveyExampleAnswers",
                columns: new[] { "ProfileId", "Purpose" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyExampleAnswers");

            migrationBuilder.AddColumn<string>(
                name: "AnswersJson",
                table: "SurveyExampleProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
