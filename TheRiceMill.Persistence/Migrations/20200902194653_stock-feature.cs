using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class stockfeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LotNumber",
                table: "GatePasses");

            migrationBuilder.AddColumn<int>(
                name: "LotId",
                table: "GatePasses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LotYear",
                table: "GatePasses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lot", x => new { x.Id, x.Year });
                });

            migrationBuilder.CreateTable(
                name: "ProcessedMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    LotId = table.Column<int>(nullable: false),
                    LotYear = table.Column<int>(nullable: false),
                    BoriQuantity = table.Column<double>(nullable: false),
                    BagQuantity = table.Column<double>(nullable: false),
                    PerKG = table.Column<double>(nullable: false),
                    TotalKG = table.Column<double>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessedMaterial_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessedMaterial_Lot_LotId_LotYear",
                        columns: x => new { x.LotId, x.LotYear },
                        principalTable: "Lot",
                        principalColumns: new[] { "Id", "Year" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RateCost",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LotId = table.Column<int>(nullable: false),
                    LotYear = table.Column<int>(nullable: false),
                    LabourUnloadingAndLoading = table.Column<double>(nullable: false),
                    Freight = table.Column<double>(nullable: false),
                    PurchaseBrokery = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    RatePer40WithoutProcessing = table.Column<double>(nullable: false),
                    ProcessingExpense = table.Column<double>(nullable: false),
                    BardanaAndMisc = table.Column<double>(nullable: false),
                    GrandTotal = table.Column<double>(nullable: false),
                    RatePer40LessByProduct = table.Column<double>(nullable: false),
                    SaleBrockery = table.Column<double>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RateCost_Lot_LotId_LotYear",
                        columns: x => new { x.LotId, x.LotYear },
                        principalTable: "Lot",
                        principalColumns: new[] { "Id", "Year" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockIn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LotId = table.Column<int>(nullable: false),
                    LotYear = table.Column<int>(nullable: false),
                    BoriQuantity = table.Column<double>(nullable: false),
                    BagQuantity = table.Column<double>(nullable: false),
                    TotalKG = table.Column<double>(nullable: false),
                    GatepassTime = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockIn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockIn_Lot_LotId_LotYear",
                        columns: x => new { x.LotId, x.LotYear },
                        principalTable: "Lot",
                        principalColumns: new[] { "Id", "Year" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockOut",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    LotId = table.Column<int>(nullable: false),
                    LotYear = table.Column<int>(nullable: false),
                    BoriQuantity = table.Column<double>(nullable: false),
                    BagQuantity = table.Column<double>(nullable: false),
                    PerKG = table.Column<double>(nullable: false),
                    TotalKG = table.Column<double>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockOut_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOut_Lot_LotId_LotYear",
                        columns: x => new { x.LotId, x.LotYear },
                        principalTable: "Lot",
                        principalColumns: new[] { "Id", "Year" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GatePasses_LotId_LotYear",
                table: "GatePasses",
                columns: new[] { "LotId", "LotYear" });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedMaterial_ProductId",
                table: "ProcessedMaterial",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedMaterial_LotId_LotYear",
                table: "ProcessedMaterial",
                columns: new[] { "LotId", "LotYear" });

            migrationBuilder.CreateIndex(
                name: "IX_RateCost_LotId_LotYear",
                table: "RateCost",
                columns: new[] { "LotId", "LotYear" });

            migrationBuilder.CreateIndex(
                name: "IX_StockIn_LotId_LotYear",
                table: "StockIn",
                columns: new[] { "LotId", "LotYear" });

            migrationBuilder.CreateIndex(
                name: "IX_StockOut_ProductId",
                table: "StockOut",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOut_LotId_LotYear",
                table: "StockOut",
                columns: new[] { "LotId", "LotYear" });

            migrationBuilder.AddForeignKey(
                name: "FK_GatePasses_Lot_LotId_LotYear",
                table: "GatePasses",
                columns: new[] { "LotId", "LotYear" },
                principalTable: "Lot",
                principalColumns: new[] { "Id", "Year" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GatePasses_Lot_LotId_LotYear",
                table: "GatePasses");

            migrationBuilder.DropTable(
                name: "ProcessedMaterial");

            migrationBuilder.DropTable(
                name: "RateCost");

            migrationBuilder.DropTable(
                name: "StockIn");

            migrationBuilder.DropTable(
                name: "StockOut");

            migrationBuilder.DropTable(
                name: "Lot");

            migrationBuilder.DropIndex(
                name: "IX_GatePasses_LotId_LotYear",
                table: "GatePasses");

            migrationBuilder.DropColumn(
                name: "LotId",
                table: "GatePasses");

            migrationBuilder.DropColumn(
                name: "LotYear",
                table: "GatePasses");

            migrationBuilder.AddColumn<string>(
                name: "LotNumber",
                table: "GatePasses",
                nullable: true);
        }
    }
}
