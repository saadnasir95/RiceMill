using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class removedfieldspurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoriQuantity",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "RateBasedOn",
                table: "Purchases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BoriQuantity",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RateBasedOn",
                table: "Purchases",
                nullable: false,
                defaultValue: 0);
        }
    }
}
