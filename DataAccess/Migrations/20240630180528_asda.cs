using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class asda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User_VoteUps",
                table: "User_VoteUps");

            migrationBuilder.DropIndex(
                name: "IX_User_VoteUps_UserId",
                table: "User_VoteUps");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_VoteUps",
                table: "User_VoteUps",
                columns: new[] { "UserId", "postId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User_VoteUps",
                table: "User_VoteUps");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "User_VoteUps",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_VoteUps",
                table: "User_VoteUps",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_VoteUps_UserId",
                table: "User_VoteUps",
                column: "UserId");
        }
    }
}
