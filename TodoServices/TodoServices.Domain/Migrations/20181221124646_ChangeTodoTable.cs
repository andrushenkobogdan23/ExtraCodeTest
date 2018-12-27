using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoServices.Domain.Migrations
{
    public partial class ChangeTodoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "Todo",
                table: "Todos",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Todo",
                table: "Todos",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AddColumn<decimal>(
                name: "ChildCost",
                schema: "Todo",
                table: "Todos",
                type: "smallmoney",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                schema: "Todo",
                table: "Todos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_ParentId",
                schema: "Todo",
                table: "Todos",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Todos_ParentId",
                schema: "Todo",
                table: "Todos",
                column: "ParentId",
                principalSchema: "Todo",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Todos_ParentId",
                schema: "Todo",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_ParentId",
                schema: "Todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "ChildCost",
                schema: "Todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "Todo",
                table: "Todos");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "Todo",
                table: "Todos",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Todo",
                table: "Todos",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 32);
        }
    }
}
