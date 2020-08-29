using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class Update_Sales_Purchase_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BasePrice",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Freight",
                table: "Sales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BasePrice",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Freight",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Freight",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Freight",
                table: "Purchases");
        }
    }
}
