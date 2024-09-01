using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentor_Management_System.Migrations.MyDb
{
    public partial class MyDbContext_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "UserInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Course",
                table: "UserInfo");
        }
    }
}
