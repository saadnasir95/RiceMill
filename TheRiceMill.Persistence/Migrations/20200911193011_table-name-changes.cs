using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class tablenamechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GatePasses_Lot_LotId_LotYear",
                table: "GatePasses");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessedMaterial_Products_ProductId",
                table: "ProcessedMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessedMaterial_Lot_LotId_LotYear",
                table: "ProcessedMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_RateCost_Lot_LotId_LotYear",
                table: "RateCost");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIn_Lot_LotId_LotYear",
                table: "StockIn");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOut_Products_ProductId",
                table: "StockOut");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOut_Lot_LotId_LotYear",
                table: "StockOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockOut",
                table: "StockOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockIn",
                table: "StockIn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RateCost",
                table: "RateCost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessedMaterial",
                table: "ProcessedMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lot",
                table: "Lot");

            migrationBuilder.RenameTable(
                name: "StockOut",
                newName: "StockOuts");

            migrationBuilder.RenameTable(
                name: "StockIn",
                newName: "StockIns");

            migrationBuilder.RenameTable(
                name: "RateCost",
                newName: "RateCosts");

            migrationBuilder.RenameTable(
                name: "ProcessedMaterial",
                newName: "ProcessedMaterials");

            migrationBuilder.RenameTable(
                name: "Lot",
                newName: "Lots");

            migrationBuilder.RenameIndex(
                name: "IX_StockOut_LotId_LotYear",
                table: "StockOuts",
                newName: "IX_StockOuts_LotId_LotYear");

            migrationBuilder.RenameIndex(
                name: "IX_StockOut_ProductId",
                table: "StockOuts",
                newName: "IX_StockOuts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_StockIn_LotId_LotYear",
                table: "StockIns",
                newName: "IX_StockIns_LotId_LotYear");

            migrationBuilder.RenameIndex(
                name: "IX_RateCost_LotId_LotYear",
                table: "RateCosts",
                newName: "IX_RateCosts_LotId_LotYear");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessedMaterial_LotId_LotYear",
                table: "ProcessedMaterials",
                newName: "IX_ProcessedMaterials_LotId_LotYear");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessedMaterial_ProductId",
                table: "ProcessedMaterials",
                newName: "IX_ProcessedMaterials_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockOuts",
                table: "StockOuts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockIns",
                table: "StockIns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RateCosts",
                table: "RateCosts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessedMaterials",
                table: "ProcessedMaterials",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lots",
                table: "Lots",
                columns: new[] { "Id", "Year" });

            migrationBuilder.AddForeignKey(
                name: "FK_GatePasses_Lots_LotId_LotYear",
                table: "GatePasses",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessedMaterials_Products_ProductId",
                table: "ProcessedMaterials",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessedMaterials_Lots_LotId_LotYear",
                table: "ProcessedMaterials",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RateCosts_Lots_LotId_LotYear",
                table: "RateCosts",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIns_Lots_LotId_LotYear",
                table: "StockIns",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOuts_Products_ProductId",
                table: "StockOuts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOuts_Lots_LotId_LotYear",
                table: "StockOuts",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GatePasses_Lots_LotId_LotYear",
                table: "GatePasses");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessedMaterials_Products_ProductId",
                table: "ProcessedMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessedMaterials_Lots_LotId_LotYear",
                table: "ProcessedMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_RateCosts_Lots_LotId_LotYear",
                table: "RateCosts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIns_Lots_LotId_LotYear",
                table: "StockIns");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOuts_Products_ProductId",
                table: "StockOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOuts_Lots_LotId_LotYear",
                table: "StockOuts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockOuts",
                table: "StockOuts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockIns",
                table: "StockIns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RateCosts",
                table: "RateCosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessedMaterials",
                table: "ProcessedMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lots",
                table: "Lots");

            migrationBuilder.RenameTable(
                name: "StockOuts",
                newName: "StockOut");

            migrationBuilder.RenameTable(
                name: "StockIns",
                newName: "StockIn");

            migrationBuilder.RenameTable(
                name: "RateCosts",
                newName: "RateCost");

            migrationBuilder.RenameTable(
                name: "ProcessedMaterials",
                newName: "ProcessedMaterial");

            migrationBuilder.RenameTable(
                name: "Lots",
                newName: "Lot");

            migrationBuilder.RenameIndex(
                name: "IX_StockOuts_LotId_LotYear",
                table: "StockOut",
                newName: "IX_StockOut_LotId_LotYear");

            migrationBuilder.RenameIndex(
                name: "IX_StockOuts_ProductId",
                table: "StockOut",
                newName: "IX_StockOut_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_StockIns_LotId_LotYear",
                table: "StockIn",
                newName: "IX_StockIn_LotId_LotYear");

            migrationBuilder.RenameIndex(
                name: "IX_RateCosts_LotId_LotYear",
                table: "RateCost",
                newName: "IX_RateCost_LotId_LotYear");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessedMaterials_LotId_LotYear",
                table: "ProcessedMaterial",
                newName: "IX_ProcessedMaterial_LotId_LotYear");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessedMaterials_ProductId",
                table: "ProcessedMaterial",
                newName: "IX_ProcessedMaterial_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockOut",
                table: "StockOut",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockIn",
                table: "StockIn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RateCost",
                table: "RateCost",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessedMaterial",
                table: "ProcessedMaterial",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lot",
                table: "Lot",
                columns: new[] { "Id", "Year" });

            migrationBuilder.AddForeignKey(
                name: "FK_GatePasses_Lot_LotId_LotYear",
                table: "GatePasses",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lot",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessedMaterial_Products_ProductId",
                table: "ProcessedMaterial",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessedMaterial_Lot_LotId_LotYear",
                table: "ProcessedMaterial",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lot",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RateCost_Lot_LotId_LotYear",
                table: "RateCost",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lot",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIn_Lot_LotId_LotYear",
                table: "StockIn",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lot",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOut_Products_ProductId",
                table: "StockOut",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOut_Lot_LotId_LotYear",
                table: "StockOut",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lot",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
