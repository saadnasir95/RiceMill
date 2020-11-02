using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class addedvoucheranddetailentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    DetailType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoucherDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    PartyId = table.Column<int>(nullable: false),
                    SaleId = table.Column<int>(nullable: true),
                    PurchaseId = table.Column<int>(nullable: true),
                    Debit = table.Column<double>(nullable: false),
                    Credit = table.Column<double>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    VoucherId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherDetail_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoucherDetail_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherDetail_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherDetail_Voucher_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Voucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoucherDetail_PartyId",
                table: "VoucherDetail",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherDetail_PurchaseId",
                table: "VoucherDetail",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherDetail_SaleId",
                table: "VoucherDetail",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherDetail_VoucherId",
                table: "VoucherDetail",
                column: "VoucherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoucherDetail");

            migrationBuilder.DropTable(
                name: "Voucher");
        }
    }
}
