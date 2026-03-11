using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWebhookLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebhookLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RawBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingResult = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Error = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebhookLogs_ExternalId",
                table: "WebhookLogs",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookLogs_OrderId",
                table: "WebhookLogs",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookLogs_ReceivedAt",
                table: "WebhookLogs",
                column: "ReceivedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebhookLogs");
        }
    }
}
