using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elderly_System.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateActivityModelRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Employees_EmployeeId",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Activities",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_EmployeeId",
                table: "Activities",
                newName: "IX_Activities_AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_AdminId",
                table: "Activities",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_AdminId",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Activities",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_AdminId",
                table: "Activities",
                newName: "IX_Activities_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Employees_EmployeeId",
                table: "Activities",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
