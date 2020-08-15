using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class LotBiltyNumberInGatepass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BiltyNumber",
                table: "GatePasses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotNumber",
                table: "GatePasses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiltyNumber",
                table: "GatePasses");

            migrationBuilder.DropColumn(
                name: "LotNumber",
                table: "GatePasses");
        }
    }
}
