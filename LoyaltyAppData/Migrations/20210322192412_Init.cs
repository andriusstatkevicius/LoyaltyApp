using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LoyaltyAppData.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    DateOfRegistration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyCards",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(nullable: false),
                    IssuingDate = table.Column<DateTime>(nullable: false),
                    ValidUntilDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyCards", x => x.Number);
                    table.ForeignKey(
                        name: "FK_LoyaltyCards_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyBalanceTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    LoyaltyPointsAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyBalanceTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoyaltyBalanceTransactions_LoyaltyCards_CardNumber",
                        column: x => x.CardNumber,
                        principalTable: "LoyaltyCards",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoyaltyBalanceTransactions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Name_Surname_Email",
                table: "Clients",
                columns: new[] { "Name", "Surname", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyBalanceTransactions_CardNumber",
                table: "LoyaltyBalanceTransactions",
                column: "CardNumber");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyBalanceTransactions_ClientId",
                table: "LoyaltyBalanceTransactions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyCards_ClientId",
                table: "LoyaltyCards",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoyaltyBalanceTransactions");

            migrationBuilder.DropTable(
                name: "LoyaltyCards");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
