using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class budgetType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SET_Activities_SET_Categories_CategoryId",
                table: "SET_Activities");

            migrationBuilder.AddColumn<int>(
                name: "BudgetType",
                table: "SET_Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "SET_Activities",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SET_Activities_SET_Categories_CategoryId",
                table: "SET_Activities",
                column: "CategoryId",
                principalTable: "SET_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SET_Activities_SET_Categories_CategoryId",
                table: "SET_Activities");

            migrationBuilder.DropColumn(
                name: "BudgetType",
                table: "SET_Categories");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "SET_Activities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SET_Activities_SET_Categories_CategoryId",
                table: "SET_Activities",
                column: "CategoryId",
                principalTable: "SET_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
