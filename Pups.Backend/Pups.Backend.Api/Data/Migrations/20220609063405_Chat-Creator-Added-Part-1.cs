using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pups.Backend.Api.Data.Migrations
{
    public partial class ChatCreatorAddedPart1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                table: "Chat",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chat_creator_id",
                table: "Chat",
                column: "creator_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_User",
                table: "Chat",
                column: "creator_id",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_User",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_creator_id",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "creator_id",
                table: "Chat");
        }
    }
}
