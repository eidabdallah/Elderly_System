using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elderly_System.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDrugPlanModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrugPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElderlyId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DailyIntake = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrugPlans_Elderlies_ElderlyId",
                        column: x => x.ElderlyId,
                        principalTable: "Elderlies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrugPlans_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugPlans_ElderlyId",
                table: "DrugPlans",
                column: "ElderlyId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugPlans_MedicineId",
                table: "DrugPlans",
                column: "MedicineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugPlans");
        }
    }
}
