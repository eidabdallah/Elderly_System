using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elderly_System.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSponsorDegree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Sponsors");

            migrationBuilder.AddColumn<int>(
                name: "Degree",
                table: "Sponsors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Sponsors");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Sponsors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
