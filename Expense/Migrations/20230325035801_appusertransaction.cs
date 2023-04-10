using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense.Migrations
{
    public partial class appusertransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Transcations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Transcations_AppUserId",
                table: "Transcations",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transcations_AspNetUsers_AppUserId",
                table: "Transcations",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transcations_AspNetUsers_AppUserId",
                table: "Transcations");

            migrationBuilder.DropIndex(
                name: "IX_Transcations_AppUserId",
                table: "Transcations");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Transcations");
        }
    }
}
