using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.App.Migrations
{
    public partial class addedSettingSocialNetworkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialNetworks_Settings_SettingId",
                table: "SocialNetworks");

            migrationBuilder.DropIndex(
                name: "IX_SocialNetworks_SettingId",
                table: "SocialNetworks");

            migrationBuilder.DropColumn(
                name: "SettingId",
                table: "SocialNetworks");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "NoticeBoards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoticeBoards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SettingSocialNetworks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SettingId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingSocialNetworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingSocialNetworks_Settings_SettingId",
                        column: x => x.SettingId,
                        principalTable: "Settings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SettingSocialNetworks_SettingId",
                table: "SettingSocialNetworks",
                column: "SettingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoticeBoards");

            migrationBuilder.DropTable(
                name: "SettingSocialNetworks");

            migrationBuilder.AddColumn<int>(
                name: "SettingId",
                table: "SocialNetworks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialNetworks_SettingId",
                table: "SocialNetworks",
                column: "SettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialNetworks_Settings_SettingId",
                table: "SocialNetworks",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
