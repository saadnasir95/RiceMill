using Microsoft.EntityFrameworkCore.Migrations;

namespace TheRiceMill.Persistence.Migrations
{
    public partial class headsdbchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Head1_Head2_Head2Id",
                table: "Head1");

            migrationBuilder.DropForeignKey(
                name: "FK_Head2_Head3_Head3Id",
                table: "Head2");

            migrationBuilder.DropForeignKey(
                name: "FK_Head3_Head4_Head4Id",
                table: "Head3");

            migrationBuilder.DropForeignKey(
                name: "FK_Head4_Head5_Head5Id",
                table: "Head4");

            migrationBuilder.DropIndex(
                name: "IX_Head4_Head5Id",
                table: "Head4");

            migrationBuilder.DropIndex(
                name: "IX_Head3_Head4Id",
                table: "Head3");

            migrationBuilder.DropIndex(
                name: "IX_Head2_Head3Id",
                table: "Head2");

            migrationBuilder.DropIndex(
                name: "IX_Head1_Head2Id",
                table: "Head1");

            migrationBuilder.DropColumn(
                name: "Head5Id",
                table: "Head4");

            migrationBuilder.DropColumn(
                name: "Head4Id",
                table: "Head3");

            migrationBuilder.DropColumn(
                name: "Head3Id",
                table: "Head2");

            migrationBuilder.DropColumn(
                name: "Head2Id",
                table: "Head1");

            migrationBuilder.AddColumn<int>(
                name: "Head4Id",
                table: "Head5",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Head3Id",
                table: "Head4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Head2Id",
                table: "Head3",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Head1Id",
                table: "Head2",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Head5_Head4Id",
                table: "Head5",
                column: "Head4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Head4_Head3Id",
                table: "Head4",
                column: "Head3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Head3_Head2Id",
                table: "Head3",
                column: "Head2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Head2_Head1Id",
                table: "Head2",
                column: "Head1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Head2_Head1_Head1Id",
                table: "Head2",
                column: "Head1Id",
                principalTable: "Head1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Head3_Head2_Head2Id",
                table: "Head3",
                column: "Head2Id",
                principalTable: "Head2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Head4_Head3_Head3Id",
                table: "Head4",
                column: "Head3Id",
                principalTable: "Head3",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Head5_Head4_Head4Id",
                table: "Head5",
                column: "Head4Id",
                principalTable: "Head4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Head2_Head1_Head1Id",
                table: "Head2");

            migrationBuilder.DropForeignKey(
                name: "FK_Head3_Head2_Head2Id",
                table: "Head3");

            migrationBuilder.DropForeignKey(
                name: "FK_Head4_Head3_Head3Id",
                table: "Head4");

            migrationBuilder.DropForeignKey(
                name: "FK_Head5_Head4_Head4Id",
                table: "Head5");

            migrationBuilder.DropIndex(
                name: "IX_Head5_Head4Id",
                table: "Head5");

            migrationBuilder.DropIndex(
                name: "IX_Head4_Head3Id",
                table: "Head4");

            migrationBuilder.DropIndex(
                name: "IX_Head3_Head2Id",
                table: "Head3");

            migrationBuilder.DropIndex(
                name: "IX_Head2_Head1Id",
                table: "Head2");

            migrationBuilder.DropColumn(
                name: "Head4Id",
                table: "Head5");

            migrationBuilder.DropColumn(
                name: "Head3Id",
                table: "Head4");

            migrationBuilder.DropColumn(
                name: "Head2Id",
                table: "Head3");

            migrationBuilder.DropColumn(
                name: "Head1Id",
                table: "Head2");

            migrationBuilder.AddColumn<int>(
                name: "Head5Id",
                table: "Head4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Head4Id",
                table: "Head3",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Head3Id",
                table: "Head2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Head2Id",
                table: "Head1",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Head4_Head5Id",
                table: "Head4",
                column: "Head5Id");

            migrationBuilder.CreateIndex(
                name: "IX_Head3_Head4Id",
                table: "Head3",
                column: "Head4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Head2_Head3Id",
                table: "Head2",
                column: "Head3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Head1_Head2Id",
                table: "Head1",
                column: "Head2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Head1_Head2_Head2Id",
                table: "Head1",
                column: "Head2Id",
                principalTable: "Head2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Head2_Head3_Head3Id",
                table: "Head2",
                column: "Head3Id",
                principalTable: "Head3",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Head3_Head4_Head4Id",
                table: "Head3",
                column: "Head4Id",
                principalTable: "Head4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Head4_Head5_Head5Id",
                table: "Head4",
                column: "Head5Id",
                principalTable: "Head5",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
