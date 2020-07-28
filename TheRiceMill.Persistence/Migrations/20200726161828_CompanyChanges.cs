using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class CompanyChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransactions_Companies_CompanyId",
                table: "BankTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_GatePasses_Companies_CompanyId",
                table: "GatePasses");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Companies_CompanyId",
                table: "Ledgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Companies_CompanyId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Companies_CompanyId",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Sales",
                newName: "PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_CompanyId",
                table: "Sales",
                newName: "IX_Sales_PartyId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Purchases",
                newName: "PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_CompanyId",
                table: "Purchases",
                newName: "IX_Purchases_PartyId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Ledgers",
                newName: "PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_Ledgers_CompanyId",
                table: "Ledgers",
                newName: "IX_Ledgers_PartyId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "GatePasses",
                newName: "PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_GatePasses_CompanyId",
                table: "GatePasses",
                newName: "IX_GatePasses_PartyId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "BankTransactions",
                newName: "PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_BankTransactions_CompanyId",
                table: "BankTransactions",
                newName: "IX_BankTransactions_PartyId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "PartyId",
                table: "Sales",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_PartyId",
                table: "Sales",
                newName: "IX_Sales_CompanyId");

            migrationBuilder.RenameColumn(
                name: "PartyId",
                table: "Purchases",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_PartyId",
                table: "Purchases",
                newName: "IX_Purchases_CompanyId");

            migrationBuilder.RenameColumn(
                name: "PartyId",
                table: "Ledgers",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Ledgers_PartyId",
                table: "Ledgers",
                newName: "IX_Ledgers_CompanyId");

            migrationBuilder.RenameColumn(
                name: "PartyId",
                table: "GatePasses",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_GatePasses_PartyId",
                table: "GatePasses",
                newName: "IX_GatePasses_CompanyId");

            migrationBuilder.RenameColumn(
                name: "PartyId",
                table: "BankTransactions",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_BankTransactions_PartyId",
                table: "BankTransactions",
                newName: "IX_BankTransactions_CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransactions_Companies_CompanyId",
                table: "BankTransactions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GatePasses_Companies_CompanyId",
                table: "GatePasses",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Companies_CompanyId",
                table: "Ledgers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Companies_CompanyId",
                table: "Purchases",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Companies_CompanyId",
                table: "Sales",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
