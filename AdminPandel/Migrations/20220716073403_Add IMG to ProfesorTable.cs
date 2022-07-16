using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminPandel.Migrations
{
    public partial class AddIMGtoProfesorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgFilePath",
                table: "Profesors",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgFilePath",
                table: "Profesors");
        }
    }
}
