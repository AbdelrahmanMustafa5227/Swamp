using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class asd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ToId",
                table: "friendRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FromId",
                table: "friendRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_friendRequests_FromId",
                table: "friendRequests",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_friendRequests_ToId",
                table: "friendRequests",
                column: "ToId");

            migrationBuilder.AddForeignKey(
                name: "FK_friendRequests_AspNetUsers_FromId",
                table: "friendRequests",
                column: "FromId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_friendRequests_AspNetUsers_ToId",
                table: "friendRequests",
                column: "ToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_friendRequests_AspNetUsers_FromId",
                table: "friendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_friendRequests_AspNetUsers_ToId",
                table: "friendRequests");

            migrationBuilder.DropIndex(
                name: "IX_friendRequests_FromId",
                table: "friendRequests");

            migrationBuilder.DropIndex(
                name: "IX_friendRequests_ToId",
                table: "friendRequests");

            migrationBuilder.AlterColumn<string>(
                name: "ToId",
                table: "friendRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FromId",
                table: "friendRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
