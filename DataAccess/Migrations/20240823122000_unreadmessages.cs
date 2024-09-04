using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class unreadmessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "unreadMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unreadMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_unreadMessages_AspNetUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_unreadMessages_AspNetUsers_ToId",
                        column: x => x.ToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_unreadMessages_FromId",
                table: "unreadMessages",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_unreadMessages_ToId",
                table: "unreadMessages",
                column: "ToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "unreadMessages");
        }
    }
}
