using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elderly_System.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateContactMessageNameModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_contactMessages",
                table: "contactMessages");

            migrationBuilder.RenameTable(
                name: "contactMessages",
                newName: "ContactMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactMessages",
                table: "ContactMessages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactMessages",
                table: "ContactMessages");

            migrationBuilder.RenameTable(
                name: "ContactMessages",
                newName: "contactMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contactMessages",
                table: "contactMessages",
                column: "Id");
        }
    }
}
