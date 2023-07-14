using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.App.Migrations
{
    public partial class addedFacultytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "Teachers");

            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_FacultyId",
                table: "Teachers",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Faculties_FacultyId",
                table: "Teachers",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Faculties_FacultyId",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_FacultyId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Teachers");

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
