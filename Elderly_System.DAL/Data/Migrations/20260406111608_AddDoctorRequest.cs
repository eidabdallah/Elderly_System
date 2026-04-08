using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elderly_System.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoctorId",
                table: "Elderlies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DoctorChangeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElderlyId = table.Column<int>(type: "int", nullable: false),
                    RequestedDoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorChangeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorChangeRequests_Doctors_RequestedDoctorId",
                        column: x => x.RequestedDoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorChangeRequests_Elderlies_ElderlyId",
                        column: x => x.ElderlyId,
                        principalTable: "Elderlies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Elderlies_DoctorId",
                table: "Elderlies",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorChangeRequests_ElderlyId",
                table: "DoctorChangeRequests",
                column: "ElderlyId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorChangeRequests_RequestedDoctorId",
                table: "DoctorChangeRequests",
                column: "RequestedDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Elderlies_Doctors_DoctorId",
                table: "Elderlies",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elderlies_Doctors_DoctorId",
                table: "Elderlies");

            migrationBuilder.DropTable(
                name: "DoctorChangeRequests");

            migrationBuilder.DropIndex(
                name: "IX_Elderlies_DoctorId",
                table: "Elderlies");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Elderlies");
        }
    }
}
