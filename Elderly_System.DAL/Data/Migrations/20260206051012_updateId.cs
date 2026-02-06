using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elderly_System.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ElderlyVisitors",
                table: "ElderlyVisitors");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ElderlyVisitors",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElderlyVisitors",
                table: "ElderlyVisitors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ElderlyVisitors_ElderlyId",
                table: "ElderlyVisitors",
                column: "ElderlyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ElderlyVisitors",
                table: "ElderlyVisitors");

            migrationBuilder.DropIndex(
                name: "IX_ElderlyVisitors_ElderlyId",
                table: "ElderlyVisitors");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ElderlyVisitors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElderlyVisitors",
                table: "ElderlyVisitors",
                columns: new[] { "ElderlyId", "VisitorId" });
        }
    }
}
