using Microsoft.EntityFrameworkCore.Migrations;

namespace Owl.Data.Migrations
{
    public partial class RemoveUserEmailFromPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Post");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Post",
                nullable: true);
        }
    }
}
