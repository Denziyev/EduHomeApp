using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.App.Migrations
{
    public partial class updatedAboutWelcomeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AboutWelcomes",
                newName: "Description2");

            migrationBuilder.AddColumn<string>(
                name: "Description1",
                table: "AboutWelcomes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description1",
                table: "AboutWelcomes");

            migrationBuilder.RenameColumn(
                name: "Description2",
                table: "AboutWelcomes",
                newName: "Description");
        }
    }
}
