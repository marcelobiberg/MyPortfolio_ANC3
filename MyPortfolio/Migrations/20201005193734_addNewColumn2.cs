using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPortfolio.Migrations
{
    public partial class addNewColumn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubUrl",
                table: "Projetos");

            migrationBuilder.AddColumn<string>(
                name: "GitUrl",
                table: "Projetos",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitUrl",
                table: "Projetos");

            migrationBuilder.AddColumn<string>(
                name: "GithubUrl",
                table: "Projetos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
