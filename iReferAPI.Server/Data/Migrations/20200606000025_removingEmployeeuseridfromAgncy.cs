using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iReferAPI.Server.Data.Migrations
{
    public partial class removingEmployeeuseridfromAgncy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgencyInvitations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(maxLength: 40, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    DateInvited = table.Column<DateTime>(type: "date", nullable: true),
                    IsSent = table.Column<bool>(nullable: false),
                    IsViewed = table.Column<bool>(nullable: false),
                    HasSubscribed = table.Column<bool>(nullable: false),
                    AgencyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgencyInvitations_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgencyInvitations_AgencyId",
                table: "AgencyInvitations",
                column: "AgencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgencyInvitations");
        }
    }
}
