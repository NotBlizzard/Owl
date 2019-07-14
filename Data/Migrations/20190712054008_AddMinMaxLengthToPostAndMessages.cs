using Microsoft.EntityFrameworkCore.Migrations;

namespace Owl.Data.Migrations
{
    public partial class AddMinMaxLengthToPostAndMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MessageData",
                table: "Message",
                maxLength: 100000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MessageData",
                table: "Message",
                maxLength: 100000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100000);
        }
    }
}
