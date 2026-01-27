using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elderly_System.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateVistorAndElderlyVistor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElderlySponsor_Elderly_ElderlyId",
                table: "ElderlySponsor");

            migrationBuilder.DropForeignKey(
                name: "FK_ElderlySponsor_Sponsors_SponsorId",
                table: "ElderlySponsor");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperience_Employees_EmployeeId",
                table: "WorkExperience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkExperience",
                table: "WorkExperience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElderlySponsor",
                table: "ElderlySponsor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Elderly",
                table: "Elderly");

            migrationBuilder.RenameTable(
                name: "WorkExperience",
                newName: "WorkExperiences");

            migrationBuilder.RenameTable(
                name: "ElderlySponsor",
                newName: "ElderlySponsors");

            migrationBuilder.RenameTable(
                name: "Elderly",
                newName: "Elderlies");

            migrationBuilder.RenameIndex(
                name: "IX_WorkExperience_EmployeeId",
                table: "WorkExperiences",
                newName: "IX_WorkExperiences_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_ElderlySponsor_SponsorId",
                table: "ElderlySponsors",
                newName: "IX_ElderlySponsors_SponsorId");

            migrationBuilder.RenameIndex(
                name: "IX_Elderly_NationalId",
                table: "Elderlies",
                newName: "IX_Elderlies_NationalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkExperiences",
                table: "WorkExperiences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElderlySponsors",
                table: "ElderlySponsors",
                columns: new[] { "ElderlyId", "SponsorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Elderlies",
                table: "Elderlies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElderlyVisitors",
                columns: table => new
                {
                    ElderlyId = table.Column<int>(type: "int", nullable: false),
                    VisitorId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElderlyVisitors", x => new { x.ElderlyId, x.VisitorId });
                    table.ForeignKey(
                        name: "FK_ElderlyVisitors_Elderlies_ElderlyId",
                        column: x => x.ElderlyId,
                        principalTable: "Elderlies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElderlyVisitors_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ElderlyVisitors_VisitorId",
                table: "ElderlyVisitors",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_Phone",
                table: "Visitors",
                column: "Phone",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ElderlySponsors_Elderlies_ElderlyId",
                table: "ElderlySponsors",
                column: "ElderlyId",
                principalTable: "Elderlies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ElderlySponsors_Sponsors_SponsorId",
                table: "ElderlySponsors",
                column: "SponsorId",
                principalTable: "Sponsors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperiences_Employees_EmployeeId",
                table: "WorkExperiences",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElderlySponsors_Elderlies_ElderlyId",
                table: "ElderlySponsors");

            migrationBuilder.DropForeignKey(
                name: "FK_ElderlySponsors_Sponsors_SponsorId",
                table: "ElderlySponsors");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperiences_Employees_EmployeeId",
                table: "WorkExperiences");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ElderlyVisitors");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkExperiences",
                table: "WorkExperiences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElderlySponsors",
                table: "ElderlySponsors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Elderlies",
                table: "Elderlies");

            migrationBuilder.RenameTable(
                name: "WorkExperiences",
                newName: "WorkExperience");

            migrationBuilder.RenameTable(
                name: "ElderlySponsors",
                newName: "ElderlySponsor");

            migrationBuilder.RenameTable(
                name: "Elderlies",
                newName: "Elderly");

            migrationBuilder.RenameIndex(
                name: "IX_WorkExperiences_EmployeeId",
                table: "WorkExperience",
                newName: "IX_WorkExperience_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_ElderlySponsors_SponsorId",
                table: "ElderlySponsor",
                newName: "IX_ElderlySponsor_SponsorId");

            migrationBuilder.RenameIndex(
                name: "IX_Elderlies_NationalId",
                table: "Elderly",
                newName: "IX_Elderly_NationalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkExperience",
                table: "WorkExperience",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElderlySponsor",
                table: "ElderlySponsor",
                columns: new[] { "ElderlyId", "SponsorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Elderly",
                table: "Elderly",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ElderlySponsor_Elderly_ElderlyId",
                table: "ElderlySponsor",
                column: "ElderlyId",
                principalTable: "Elderly",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ElderlySponsor_Sponsors_SponsorId",
                table: "ElderlySponsor",
                column: "SponsorId",
                principalTable: "Sponsors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperience_Employees_EmployeeId",
                table: "WorkExperience",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
