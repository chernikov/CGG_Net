using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSurveyExampleProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyExampleProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NameUk = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AnswersJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyExampleProfiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyExampleProfiles_SurveyType",
                table: "SurveyExampleProfiles",
                column: "SurveyType");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyExampleProfiles_SurveyType_Slug",
                table: "SurveyExampleProfiles",
                columns: new[] { "SurveyType", "Slug" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyExampleProfiles");
        }
    }
}
