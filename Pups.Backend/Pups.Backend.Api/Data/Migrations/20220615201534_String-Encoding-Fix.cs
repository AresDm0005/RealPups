using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pups.Backend.Api.Data.Migrations
{
    public partial class StringEncodingFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "payload",
                table: "Message",
                type: "nvarchar(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "payload",
                table: "Message",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)");
        }
    }
}
