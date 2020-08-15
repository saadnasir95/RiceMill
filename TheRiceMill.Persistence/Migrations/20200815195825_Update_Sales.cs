using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class Update_Sales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Parties_PartyId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Vehicles_VehicleId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ActualBagWeight",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BagWeight",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BiltyNumber",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ExpectedBagWeight",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ExpectedEmptyBagWeight",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "KandaWeight",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PercentCommission",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "RatePerKg",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "RatePerMaund",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TotalActualBagWeight",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "TotalExpectedEmptyBagWeight",
                table: "Sales",
                newName: "Rate");

            migrationBuilder.RenameColumn(
                name: "TotalExpectedBagWeight",
                table: "Sales",
                newName: "BoriQuantity");

            migrationBuilder.RenameColumn(
                name: "CheckOut",
                table: "Sales",
                newName: "Date");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "Sales",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Sales",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PartyId",
                table: "Sales",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "RateBasedOn",
                table: "Sales",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Parties_PartyId",
                table: "Sales",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Vehicles_VehicleId",
                table: "Sales",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Parties_PartyId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Vehicles_VehicleId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "RateBasedOn",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "Sales",
                newName: "TotalExpectedEmptyBagWeight");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Sales",
                newName: "CheckOut");

            migrationBuilder.RenameColumn(
                name: "BoriQuantity",
                table: "Sales",
                newName: "TotalExpectedBagWeight");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "Sales",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Sales",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PartyId",
                table: "Sales",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ActualBagWeight",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BagWeight",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BasePrice",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "BiltyNumber",
                table: "Sales",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedBagWeight",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedEmptyBagWeight",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "KandaWeight",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentCommission",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RatePerKg",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RatePerMaund",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalActualBagWeight",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Parties_PartyId",
                table: "Sales",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Vehicles_VehicleId",
                table: "Sales",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
