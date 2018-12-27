using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoServices.Domain.Migrations
{
    public partial class RevertchildCostMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.DropColumn(
                name: "ChildCost",
                schema: "Todo",
                table: "Todos");

        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.AddColumn<decimal>(
                name: "ChildCost",
                schema: "Todo",
                table: "Todos",
                type: "smallmoney",
                nullable: true);

        }
    }
}
