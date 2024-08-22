using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LovesInsteadofLikesandDislikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "posts");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "posts",
                newName: "Loves");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Loves",
                table: "posts",
                newName: "Likes");

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "posts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
