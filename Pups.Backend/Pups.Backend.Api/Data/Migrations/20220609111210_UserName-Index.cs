using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pups.Backend.Api.Data.Migrations
{
    public partial class UserNameIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_username",
                table: "User",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_username",
                table: "User");
        }
    }
}
