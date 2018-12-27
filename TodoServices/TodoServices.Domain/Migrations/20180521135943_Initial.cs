using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TodoServices.Domain.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Todo");

            migrationBuilder.CreateTable(
                name: "Todos",
                schema: "Todo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<int>(nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "date", nullable: true),
                    Cost = table.Column<decimal>(type: "smallmoney", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: false),
                    EstimatedCost = table.Column<decimal>(type: "smallmoney", nullable: false),
                    PerformerId = table.Column<int>(nullable: true),
                    Priority = table.Column<byte>(nullable: false),
                    Title = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_AuthorId",
                schema: "Todo",
                table: "Todos",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_PerformerId",
                schema: "Todo",
                table: "Todos",
                column: "PerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_Priority_CreateDate_CompleteDate",
                schema: "Todo",
                table: "Todos",
                columns: new[] { "Priority", "CreateDate", "CompleteDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos",
                schema: "Todo");
        }
    }
}
