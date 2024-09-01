using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentor_Management_System.Migrations.MyDb
{
    public partial class MyDbContext_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_First_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    User_Last_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    User_Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    User_Phone = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    User_Info_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    School_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    School_College = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    School_Marks = table.Column<int>(type: "int", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.User_Info_Id);
                    table.ForeignKey(
                        name: "FK_UserInfo_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_User_Id",
                table: "UserInfo",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
