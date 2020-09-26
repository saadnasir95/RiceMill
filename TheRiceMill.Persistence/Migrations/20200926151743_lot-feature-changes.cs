using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class lotfeaturechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GatePasses_Lots_LotId_LotYear",
                table: "GatePasses");

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
                name: "FK_StockOuts_Lots_LotId_LotYear",
                table: "StockOuts");

            migrationBuilder.DropIndex(
                name: "IX_StockOuts_LotId_LotYear",
                table: "StockOuts");

            migrationBuilder.DropIndex(
                name: "IX_StockIns_LotId_LotYear",
                table: "StockIns");

            migrationBuilder.DropIndex(
                name: "IX_RateCosts_LotId_LotYear",
                table: "RateCosts");

            migrationBuilder.DropIndex(
                name: "IX_ProcessedMaterials_LotId_LotYear",
                table: "ProcessedMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lots",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_GatePasses_LotId_LotYear",
                table: "GatePasses");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "StockOuts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "StockIns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "RateCosts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ProcessedMaterials",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lots",
                table: "Lots",
                columns: new[] { "Id", "Year", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_StockOuts_LotId_LotYear_CompanyId",
                table: "StockOuts",
                columns: new[] { "LotId", "LotYear", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_StockIns_LotId_LotYear_CompanyId",
                table: "StockIns",
                columns: new[] { "LotId", "LotYear", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_RateCosts_LotId_LotYear_CompanyId",
                table: "RateCosts",
                columns: new[] { "LotId", "LotYear", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedMaterials_LotId_LotYear_CompanyId",
                table: "ProcessedMaterials",
                columns: new[] { "LotId", "LotYear", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_GatePasses_LotId_LotYear_CompanyId",
                table: "GatePasses",
                columns: new[] { "LotId", "LotYear", "CompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GatePasses_Lots_LotId_LotYear_CompanyId",
                table: "GatePasses",
                columns: new[] { "LotId", "LotYear", "CompanyId" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year", "CompanyId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessedMaterials_Lots_LotId_LotYear_CompanyId",
                table: "ProcessedMaterials",
                columns: new[] { "LotId", "LotYear", "CompanyId" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year", "CompanyId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RateCosts_Lots_LotId_LotYear_CompanyId",
                table: "RateCosts",
                columns: new[] { "LotId", "LotYear", "CompanyId" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year", "CompanyId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIns_Lots_LotId_LotYear_CompanyId",
                table: "StockIns",
                columns: new[] { "LotId", "LotYear", "CompanyId" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year", "CompanyId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockOuts_Lots_LotId_LotYear_CompanyId",
                table: "StockOuts",
                columns: new[] { "LotId", "LotYear", "CompanyId" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year", "CompanyId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GatePasses_Lots_LotId_LotYear_CompanyId",
                table: "GatePasses");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessedMaterials_Lots_LotId_LotYear_CompanyId",
                table: "ProcessedMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_RateCosts_Lots_LotId_LotYear_CompanyId",
                table: "RateCosts");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIns_Lots_LotId_LotYear_CompanyId",
                table: "StockIns");

            migrationBuilder.DropForeignKey(
                name: "FK_StockOuts_Lots_LotId_LotYear_CompanyId",
                table: "StockOuts");

            migrationBuilder.DropIndex(
                name: "IX_StockOuts_LotId_LotYear_CompanyId",
                table: "StockOuts");

            migrationBuilder.DropIndex(
                name: "IX_StockIns_LotId_LotYear_CompanyId",
                table: "StockIns");

            migrationBuilder.DropIndex(
                name: "IX_RateCosts_LotId_LotYear_CompanyId",
                table: "RateCosts");

            migrationBuilder.DropIndex(
                name: "IX_ProcessedMaterials_LotId_LotYear_CompanyId",
                table: "ProcessedMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lots",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_GatePasses_LotId_LotYear_CompanyId",
                table: "GatePasses");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "StockOuts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "StockIns");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "RateCosts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ProcessedMaterials");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lots",
                table: "Lots",
                columns: new[] { "Id", "Year" });

            migrationBuilder.CreateIndex(
                name: "IX_StockOuts_LotId_LotYear",
                table: "StockOuts",
                columns: new[] { "LotId", "LotYear" });

            migrationBuilder.CreateIndex(
                name: "IX_StockIns_LotId_LotYear",
                table: "StockIns",
                columns: new[] { "LotId", "LotYear" });

            migrationBuilder.CreateIndex(
                name: "IX_RateCosts_LotId_LotYear",
                table: "RateCosts",
                columns: new[] { "LotId", "LotYear" });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedMaterials_LotId_LotYear",
                table: "ProcessedMaterials",
                columns: new[] { "LotId", "LotYear" });

            migrationBuilder.CreateIndex(
                name: "IX_GatePasses_LotId_LotYear",
                table: "GatePasses",
                columns: new[] { "LotId", "LotYear" });

            migrationBuilder.AddForeignKey(
                name: "FK_GatePasses_Lots_LotId_LotYear",
                table: "GatePasses",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year" },
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
                name: "FK_StockOuts_Lots_LotId_LotYear",
                table: "StockOuts",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lots",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
