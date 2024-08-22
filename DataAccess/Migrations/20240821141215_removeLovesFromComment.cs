using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeLovesFromComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "loves",
                table: "comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "loves",
                table: "comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
