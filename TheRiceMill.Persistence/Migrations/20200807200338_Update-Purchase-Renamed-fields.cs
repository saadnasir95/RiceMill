using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class UpdatePurchaseRenamedfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatePerMaund",
                table: "Purchases",
                newName: "TotalMaund");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalMaund",
                table: "Purchases",
                newName: "RatePerMaund");
        }
    }
}
