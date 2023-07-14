using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.App.Migrations
{
    public partial class updatedskillstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Percent",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percent",
                table: "Skills");
        }
    }
}
