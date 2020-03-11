using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPortfolio.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projetos",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Titulo = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    Categoria = table.Column<string>(nullable: true),
                    ImgBackground = table.Column<string>(nullable: true),
                    Img01 = table.Column<string>(nullable: true),
                    Img02 = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projetos", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projetos");
        }
    }
}
