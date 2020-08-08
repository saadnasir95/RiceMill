using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class ledgerchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ledgers",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "Credit",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Ledgers");

            migrationBuilder.RenameColumn(
                name: "Debit",
                table: "Ledgers",
                newName: "Amount");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Ledgers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Ledgers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "Ledgers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ledgers",
                table: "Ledgers",
                columns: new[] { "LedgerType", "Id", "TransactionType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ledgers",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Ledgers");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Ledgers",
                newName: "Debit");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "Ledgers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Credit",
                table: "Ledgers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Ledgers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ledgers",
                table: "Ledgers",
                columns: new[] { "LedgerType", "TransactionId" });
        }
    }
}
