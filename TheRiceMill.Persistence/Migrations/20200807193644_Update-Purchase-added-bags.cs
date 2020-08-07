using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class UpdatePurchaseaddedbags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalMaund",
                table: "Purchases",
                newName: "Rate");

            migrationBuilder.AddColumn<double>(
                name: "BagQuantity",
                table: "Purchases",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BagQuantity",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "Purchases",
                newName: "TotalMaund");
        }
    }
}
