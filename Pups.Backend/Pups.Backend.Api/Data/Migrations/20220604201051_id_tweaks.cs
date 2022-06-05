using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pups.Backend.Api.Data.Migrations;

public partial class id_tweaks : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ChatStatus",
            columns: table => new
            {
                id = table.Column<int>(type: "int", nullable: false),
                title = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ChatStatus", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "ChatType",
            columns: table => new
            {
                id = table.Column<int>(type: "int", nullable: false),
                name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ChatType", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "User",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                created = table.Column<DateTime>(type: "datetime", nullable: false),
                last_seen = table.Column<DateTime>(type: "datetime", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_User", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "Chat",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                type_id = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Chat", x => x.id);
                table.ForeignKey(
                    name: "FK_Chat_ChatType",
                    column: x => x.type_id,
                    principalTable: "ChatType",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "ChatMember",
            columns: table => new
            {
                chat_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                chat_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                chat_status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ChatMember", x => new { x.chat_id, x.user_id });
                table.ForeignKey(
                    name: "FK_ChatMember_Chat",
                    column: x => x.chat_id,
                    principalTable: "Chat",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_ChatMember_ChatStatus",
                    column: x => x.chat_status,
                    principalTable: "ChatStatus",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_ChatMember_User",
                    column: x => x.user_id,
                    principalTable: "User",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "Message",
            columns: table => new
            {
                id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                chat_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                sender_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                payload = table.Column<string>(type: "text", nullable: false),
                send_at = table.Column<DateTime>(type: "datetime", nullable: false),
                checked_at = table.Column<DateTime>(type: "datetime", nullable: true),
                edited = table.Column<bool>(type: "bit", nullable: false),
                reply_to = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Message", x => x.id);
                table.ForeignKey(
                    name: "FK_Message_Chat",
                    column: x => x.chat_id,
                    principalTable: "Chat",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_Message_Message",
                    column: x => x.reply_to,
                    principalTable: "Message",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "FK_Message_User",
                    column: x => x.sender_id,
                    principalTable: "User",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Chat_type_id",
            table: "Chat",
            column: "type_id");

        migrationBuilder.CreateIndex(
            name: "IX_ChatMember_chat_status",
            table: "ChatMember",
            column: "chat_status");

        migrationBuilder.CreateIndex(
            name: "IX_ChatMember_user_id",
            table: "ChatMember",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "IX_Message_chat_id",
            table: "Message",
            column: "chat_id");

        migrationBuilder.CreateIndex(
            name: "IX_Message_reply_to",
            table: "Message",
            column: "reply_to");

        migrationBuilder.CreateIndex(
            name: "IX_Message_sender_id",
            table: "Message",
            column: "sender_id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ChatMember");

        migrationBuilder.DropTable(
            name: "Message");

        migrationBuilder.DropTable(
            name: "ChatStatus");

        migrationBuilder.DropTable(
            name: "Chat");

        migrationBuilder.DropTable(
            name: "User");

        migrationBuilder.DropTable(
            name: "ChatType");
    }
}
