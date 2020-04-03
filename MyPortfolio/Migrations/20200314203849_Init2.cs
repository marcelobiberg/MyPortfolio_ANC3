using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPortfolio.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Projetos");

            migrationBuilder.AddColumn<string>(
                name: "BackEnd",
                table: "Projetos",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BancoDados",
                table: "Projetos",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontEnd",
                table: "Projetos",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Projetos",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackEnd",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "BancoDados",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "FrontEnd",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Projetos");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Projetos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
