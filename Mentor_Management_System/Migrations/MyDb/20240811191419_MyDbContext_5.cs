using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentor_Management_System.Migrations.MyDb
{
    public partial class MyDbContext_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "APF_Challan",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Admit_Card",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TF_Challan",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WellCome_Letter",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "APF_Challan",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Admit_Card",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TF_Challan",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WellCome_Letter",
                table: "Users");
        }
    }
}
