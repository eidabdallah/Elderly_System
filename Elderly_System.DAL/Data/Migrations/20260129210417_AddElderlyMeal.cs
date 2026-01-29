using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elderly_System.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddElderlyMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElderMeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElderlyId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MealType = table.Column<int>(type: "int", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElderMeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElderMeals_Elderlies_ElderlyId",
                        column: x => x.ElderlyId,
                        principalTable: "Elderlies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElderMeals_ElderlyId_Date_MealType",
                table: "ElderMeals",
                columns: new[] { "ElderlyId", "Date", "MealType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElderMeals");
        }
    }
}
