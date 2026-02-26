using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elderly_System.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCheckListModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InOut",
                table: "CheckLists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InOut",
                table: "CheckLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
