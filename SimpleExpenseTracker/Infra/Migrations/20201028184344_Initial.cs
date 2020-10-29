using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SET_Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    Salt = table.Column<byte[]>(nullable: true),
                    ActiveAccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SET_Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountOwnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SET_Accounts_SET_Users_AccountOwnerId",
                        column: x => x.AccountOwnerId,
                        principalTable: "SET_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SET_Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Budget = table.Column<decimal>(nullable: false),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SET_Categories_SET_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "SET_Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SET_UsersAccounts",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_UsersAccounts", x => new { x.AccountId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SET_UsersAccounts_SET_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "SET_Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SET_UsersAccounts_SET_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "SET_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SET_Activities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SET_Activities_SET_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "SET_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SET_Accounts_AccountOwnerId",
                table: "SET_Accounts",
                column: "AccountOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SET_Activities_CategoryId",
                table: "SET_Activities",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SET_Categories_AccountId",
                table: "SET_Categories",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SET_Users_Email",
                table: "SET_Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SET_UsersAccounts_UserId",
                table: "SET_UsersAccounts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SET_Activities");

            migrationBuilder.DropTable(
                name: "SET_UsersAccounts");

            migrationBuilder.DropTable(
                name: "SET_Categories");

            migrationBuilder.DropTable(
                name: "SET_Accounts");

            migrationBuilder.DropTable(
                name: "SET_Users");
        }
    }
}
