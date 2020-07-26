using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class GatePassChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "GatePasses");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "GatePasses");

            migrationBuilder.RenameColumn(
                name: "TotalMaund",
                table: "GatePasses",
                newName: "WeightPerBag");

            migrationBuilder.RenameColumn(
                name: "KandaWeight",
                table: "GatePasses",
                newName: "NetWeight");

            migrationBuilder.RenameColumn(
                name: "BiltyNumber",
                table: "GatePasses",
                newName: "Broker");

            migrationBuilder.RenameColumn(
                name: "BagWeight",
                table: "GatePasses",
                newName: "Maund");

            migrationBuilder.AddColumn<double>(
                name: "BoriQuantity",
                table: "GatePasses",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "GatePasses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "GatePasses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "GatePasses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GatePasses_PurchaseId",
                table: "GatePasses",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_GatePasses_SaleId",
                table: "GatePasses",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GatePasses_Purchases_PurchaseId",
                table: "GatePasses",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GatePasses_Sales_SaleId",
                table: "GatePasses",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GatePasses_Purchases_PurchaseId",
                table: "GatePasses");

            migrationBuilder.DropForeignKey(
                name: "FK_GatePasses_Sales_SaleId",
                table: "GatePasses");

            migrationBuilder.DropIndex(
                name: "IX_GatePasses_PurchaseId",
                table: "GatePasses");

            migrationBuilder.DropIndex(
                name: "IX_GatePasses_SaleId",
                table: "GatePasses");

            migrationBuilder.DropColumn(
                name: "BoriQuantity",
                table: "GatePasses");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "GatePasses");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "GatePasses");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "GatePasses");

            migrationBuilder.RenameColumn(
                name: "WeightPerBag",
                table: "GatePasses",
                newName: "TotalMaund");

            migrationBuilder.RenameColumn(
                name: "NetWeight",
                table: "GatePasses",
                newName: "KandaWeight");

            migrationBuilder.RenameColumn(
                name: "Maund",
                table: "GatePasses",
                newName: "BagWeight");

            migrationBuilder.RenameColumn(
                name: "Broker",
                table: "GatePasses",
                newName: "BiltyNumber");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "GatePasses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "GatePasses",
                nullable: false,
                defaultValue: 0);
        }
    }
}
