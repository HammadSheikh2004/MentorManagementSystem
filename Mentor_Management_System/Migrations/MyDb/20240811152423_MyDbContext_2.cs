using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentor_Management_System.Migrations.MyDb
{
    public partial class MyDbContext_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "School_College",
                table: "UserInfo",
                newName: "College_Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "College_Name",
                table: "UserInfo",
                newName: "School_College");
        }
    }
}
