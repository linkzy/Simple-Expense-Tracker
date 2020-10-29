using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryIcon",
                table: "SET_Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryType",
                table: "SET_Categories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryIcon",
                table: "SET_Categories");

            migrationBuilder.DropColumn(
                name: "CategoryType",
                table: "SET_Categories");
        }
    }
}
