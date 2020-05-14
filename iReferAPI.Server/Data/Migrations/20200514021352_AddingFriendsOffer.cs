using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iReferAPI.Server.Data.Migrations
{
    public partial class AddingFriendsOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendOffers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(maxLength: 40, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "date", nullable: true),
                    OfferType = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 500, nullable: false),
                    DiscountRate = table.Column<float>(nullable: false),
                    NoExpiration = table.Column<bool>(nullable: false),
                    LandingPage = table.Column<string>(maxLength: 100, nullable: true),
                    SalesPhoneNumber = table.Column<string>(nullable: true),
                    AgencyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendOffers_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendOffers_AgencyId",
                table: "FriendOffers",
                column: "AgencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendOffers");
        }
    }
}
