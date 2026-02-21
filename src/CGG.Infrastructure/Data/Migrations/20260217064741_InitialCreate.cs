using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Credits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Credits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxUses = table.Column<int>(type: "int", nullable: true),
                    UsedCount = table.Column<int>(type: "int", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Credits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequiresAiAnalysis = table.Column<bool>(type: "bit", nullable: false),
                    PurposeCategory = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    DefaultLanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_Languages_DefaultLanguageId",
                        column: x => x.DefaultLanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Credits = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_User_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyQuestionOptions_SurveyQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "SurveyQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestionTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyQuestionTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveyQuestionTranslations_SurveyQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "SurveyQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AiPromptTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromptType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PromptText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SystemPrompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temperature = table.Column<double>(type: "float", nullable: true),
                    MaxTokens = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiPromptTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiPromptTemplates_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AIRecommendations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokensUsed = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIRecommendations_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestionOptionTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestionOptionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyQuestionOptionTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveyQuestionOptionTranslations_SurveyQuestionOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "SurveyQuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveySteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AiPrompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AiPromptTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SkipAiProcessing = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveySteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveySteps_AiPromptTemplates_AiPromptTemplateId",
                        column: x => x.AiPromptTemplateId,
                        principalTable: "AiPromptTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SurveySteps_SurveyQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "SurveyQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveySteps_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SurveyType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentStep = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Steps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Results = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokensUsed = table.Column<int>(type: "int", nullable: true),
                    ProcessingTimeMs = table.Column<int>(type: "int", nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyResults_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyResults_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurveyResults_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyResults_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SurveyResults_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AiLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SurveyResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PromptTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepNumber = table.Column<int>(type: "int", nullable: true),
                    PromptText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemPrompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserInput = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokensUsed = table.Column<int>(type: "int", nullable: true),
                    PromptTokens = table.Column<int>(type: "int", nullable: true),
                    CompletionTokens = table.Column<int>(type: "int", nullable: true),
                    ProcessingTimeMs = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostUsd = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiLogs_AiPromptTemplates_PromptTemplateId",
                        column: x => x.PromptTemplateId,
                        principalTable: "AiPromptTemplates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AiLogs_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AiLogs_SurveyResults_SurveyResultId",
                        column: x => x.SurveyResultId,
                        principalTable: "SurveyResults",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AiLogs_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_CreatedAt",
                table: "AiLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_MemberId",
                table: "AiLogs",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_MemberId_CreatedAt",
                table: "AiLogs",
                columns: new[] { "MemberId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_PromptTemplateId",
                table: "AiLogs",
                column: "PromptTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_RequestType",
                table: "AiLogs",
                column: "RequestType");

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_Status",
                table: "AiLogs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_SurveyResultId",
                table: "AiLogs",
                column: "SurveyResultId");

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_UserId",
                table: "AiLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AiLogs_UserId_CreatedAt",
                table: "AiLogs",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AiPromptTemplates_Category",
                table: "AiPromptTemplates",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_AiPromptTemplates_CreatedByUserId",
                table: "AiPromptTemplates",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AiPromptTemplates_IsActive",
                table: "AiPromptTemplates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_AiPromptTemplates_IsDefault",
                table: "AiPromptTemplates",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_AiPromptTemplates_PromptType",
                table: "AiPromptTemplates",
                column: "PromptType");

            migrationBuilder.CreateIndex(
                name: "IX_AiPromptTemplates_PromptType_Version",
                table: "AiPromptTemplates",
                columns: new[] { "PromptType", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_AIRecommendations_CreatedAt",
                table: "AIRecommendations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AIRecommendations_UserId",
                table: "AIRecommendations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                table: "Languages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_IsDefault",
                table: "Languages",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_Members_FamilyId",
                table: "Members",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_Code",
                table: "PromoCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionOptions_QuestionId_SortOrder",
                table: "SurveyQuestionOptions",
                columns: new[] { "QuestionId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionOptionTranslations_LanguageId",
                table: "SurveyQuestionOptionTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionOptionTranslations_OptionId_LanguageId",
                table: "SurveyQuestionOptionTranslations",
                columns: new[] { "OptionId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_IsActive",
                table: "SurveyQuestions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_PurposeCategory",
                table: "SurveyQuestions",
                column: "PurposeCategory");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_QuestionType",
                table: "SurveyQuestions",
                column: "QuestionType");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_RequiresAiAnalysis",
                table: "SurveyQuestions",
                column: "RequiresAiAnalysis");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionTranslations_LanguageId",
                table: "SurveyQuestionTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionTranslations_QuestionId_LanguageId",
                table: "SurveyQuestionTranslations",
                columns: new[] { "QuestionId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResults_CompletedAt",
                table: "SurveyResults",
                column: "CompletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResults_CurrentStep",
                table: "SurveyResults",
                column: "CurrentStep");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResults_FamilyId",
                table: "SurveyResults",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResults_LanguageId",
                table: "SurveyResults",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResults_MemberId",
                table: "SurveyResults",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResults_SurveyId",
                table: "SurveyResults",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResults_SurveyType",
                table: "SurveyResults",
                column: "SurveyType");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResults_UpdatedAt",
                table: "SurveyResults",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResults_UserId",
                table: "SurveyResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_DefaultLanguageId",
                table: "Surveys",
                column: "DefaultLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_IsActive",
                table: "Surveys",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_SurveyType",
                table: "Surveys",
                column: "SurveyType");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_SurveyType_Version",
                table: "Surveys",
                columns: new[] { "SurveyType", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveySteps_AiPromptTemplateId",
                table: "SurveySteps",
                column: "AiPromptTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveySteps_QuestionId",
                table: "SurveySteps",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveySteps_SurveyId_StepNumber",
                table: "SurveySteps",
                columns: new[] { "SurveyId", "StepNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FamilyId",
                table: "Transactions",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedAt",
                table: "User",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_FamilyId",
                table: "User",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_User_MemberId",
                table: "User",
                column: "MemberId",
                unique: true,
                filter: "[MemberId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role",
                table: "User",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_User_SchoolId",
                table: "User",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiLogs");

            migrationBuilder.DropTable(
                name: "AIRecommendations");

            migrationBuilder.DropTable(
                name: "PromoCodes");

            migrationBuilder.DropTable(
                name: "SurveyQuestionOptionTranslations");

            migrationBuilder.DropTable(
                name: "SurveyQuestionTranslations");

            migrationBuilder.DropTable(
                name: "SurveySteps");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "SurveyResults");

            migrationBuilder.DropTable(
                name: "SurveyQuestionOptions");

            migrationBuilder.DropTable(
                name: "AiPromptTemplates");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "SurveyQuestions");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
