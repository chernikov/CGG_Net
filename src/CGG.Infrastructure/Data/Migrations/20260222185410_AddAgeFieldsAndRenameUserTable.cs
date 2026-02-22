using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAgeFieldsAndRenameUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiLogs_User_UserId",
                table: "AiLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AiPromptTemplates_User_CreatedByUserId",
                table: "AiPromptTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_AIRecommendations_User_UserId",
                table: "AIRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_User_UserId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResults_User_UserId",
                table: "SurveyResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_User_UserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Families_FamilyId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Schools_SchoolId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_User_SchoolId",
                table: "Users",
                newName: "IX_Users_SchoolId");

            migrationBuilder.RenameIndex(
                name: "IX_User_Role",
                table: "Users",
                newName: "IX_Users_Role");

            migrationBuilder.RenameIndex(
                name: "IX_User_MemberId",
                table: "Users",
                newName: "IX_Users_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_User_FamilyId",
                table: "Users",
                newName: "IX_Users_FamilyId");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_User_CreatedAt",
                table: "Users",
                newName: "IX_Users_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AiLogs_Users_UserId",
                table: "AiLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AiPromptTemplates_Users_CreatedByUserId",
                table: "AiPromptTemplates",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AIRecommendations_Users_UserId",
                table: "AIRecommendations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Users_UserId",
                table: "Members",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResults_Users_UserId",
                table: "SurveyResults",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Families_FamilyId",
                table: "Users",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Schools_SchoolId",
                table: "Users",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiLogs_Users_UserId",
                table: "AiLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AiPromptTemplates_Users_CreatedByUserId",
                table: "AiPromptTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_AIRecommendations_Users_UserId",
                table: "AIRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Users_UserId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResults_Users_UserId",
                table: "SurveyResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Families_FamilyId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Schools_SchoolId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AgeAddedDate",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SchoolId",
                table: "User",
                newName: "IX_User_SchoolId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Role",
                table: "User",
                newName: "IX_User_Role");

            migrationBuilder.RenameIndex(
                name: "IX_Users_MemberId",
                table: "User",
                newName: "IX_User_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_FamilyId",
                table: "User",
                newName: "IX_User_FamilyId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CreatedAt",
                table: "User",
                newName: "IX_User_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AiLogs_User_UserId",
                table: "AiLogs",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AiPromptTemplates_User_CreatedByUserId",
                table: "AiPromptTemplates",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AIRecommendations_User_UserId",
                table: "AIRecommendations",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_User_UserId",
                table: "Members",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResults_User_UserId",
                table: "SurveyResults",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_User_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Families_FamilyId",
                table: "User",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Schools_SchoolId",
                table: "User",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
