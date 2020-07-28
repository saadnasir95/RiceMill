using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class RenameCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_Companies_PartyId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_GatePasses_Companies_PartyId",
                table: "GatePasses");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Companies_PartyId",
                table: "Ledgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Companies_PartyId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Companies_PartyId",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Parties");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parties",
                table: "Parties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_Parties_PartyId",
                table: "BankTransactions",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GatePasses_Parties_PartyId",
                table: "GatePasses",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Parties_PartyId",
                table: "Ledgers",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Parties_PartyId",
                table: "Purchases",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Parties_PartyId",
                table: "Sales",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_Parties_PartyId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_GatePasses_Parties_PartyId",
                table: "GatePasses");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Parties_PartyId",
                table: "Ledgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Parties_PartyId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Parties_PartyId",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parties",
                table: "Parties");

            migrationBuilder.RenameTable(
                name: "Parties",
                newName: "Companies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_Companies_PartyId",
                table: "BankTransactions",
                column: "PartyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GatePasses_Companies_PartyId",
                table: "GatePasses",
                column: "PartyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Companies_PartyId",
                table: "Ledgers",
                column: "PartyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Companies_PartyId",
                table: "Purchases",
                column: "PartyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Companies_PartyId",
                table: "Sales",
                column: "PartyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
