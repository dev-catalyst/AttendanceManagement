using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceManagement.Migrations
{
    public partial class ImprovedStudentBatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentBatches_AspNetUsers_Id",
                table: "StudentBatches");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "StudentBatches",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBatches_UserId",
                table: "StudentBatches",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBatches_AspNetUsers_UserId",
                table: "StudentBatches",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentBatches_AspNetUsers_UserId",
                table: "StudentBatches");

            migrationBuilder.DropIndex(
                name: "IX_StudentBatches_UserId",
                table: "StudentBatches");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StudentBatches");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBatches_AspNetUsers_Id",
                table: "StudentBatches",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
