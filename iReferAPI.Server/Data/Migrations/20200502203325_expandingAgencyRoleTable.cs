using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iReferAPI.Server.Data.Migrations
{
    public partial class expandingAgencyRoleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgencyEmplyees");

            migrationBuilder.CreateTable(
                name: "AgencyRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(maxLength: 40, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EmployeeUserID = table.Column<string>(nullable: true),
                    AgencyId = table.Column<string>(nullable: true),
                    UserRoleID = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgencyRoles_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgencyRoles_AgencyId",
                table: "AgencyRoles",
                column: "AgencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgencyRoles");

            migrationBuilder.CreateTable(
                name: "AgencyEmplyees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AgencyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeUserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    UserRoleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyEmplyees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgencyEmplyees_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgencyEmplyees_AgencyId",
                table: "AgencyEmplyees",
                column: "AgencyId");
        }
    }
}
