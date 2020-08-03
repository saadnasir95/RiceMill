using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class UpdatePurchaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Parties_PartyId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Products_ProductId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Vehicles_VehicleId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PartyId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_VehicleId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ActualBagWeight",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ActualBags",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "BagQuantity",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "BagWeight",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ExpectedBagWeight",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ExpectedEmptyBagWeight",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "KandaWeight",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PartyId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PercentCommission",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "RatePerKg",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "TotalActualBagWeight",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "TotalExpectedBagWeight",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "TotalExpectedEmptyBagWeight",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Vibration",
                table: "Purchases");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Purchases",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Products_ProductId",
                table: "Purchases",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Products_ProductId",
                table: "Purchases");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Purchases",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ActualBagWeight",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActualBags",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BagQuantity",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BagWeight",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BasePrice",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "Purchases",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedBagWeight",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedEmptyBagWeight",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "KandaWeight",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PartyId",
                table: "Purchases",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PercentCommission",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RatePerKg",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalActualBagWeight",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalExpectedBagWeight",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalExpectedEmptyBagWeight",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Purchases",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Vibration",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PartyId",
                table: "Purchases",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_VehicleId",
                table: "Purchases",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Parties_PartyId",
                table: "Purchases",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Products_ProductId",
                table: "Purchases",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Vehicles_VehicleId",
                table: "Purchases",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
