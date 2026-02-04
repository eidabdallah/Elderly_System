using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elderly_System.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDonationModelRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Employees_EmployeeId",
                table: "Donations");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Donations",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Donations_EmployeeId",
                table: "Donations",
                newName: "IX_Donations_AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Users_AdminId",
                table: "Donations",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Users_AdminId",
                table: "Donations");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Donations",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Donations_AdminId",
                table: "Donations",
                newName: "IX_Donations_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Employees_EmployeeId",
                table: "Donations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
