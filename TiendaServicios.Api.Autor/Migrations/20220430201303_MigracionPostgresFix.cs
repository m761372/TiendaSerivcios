using Microsoft.EntityFrameworkCore.Migrations;

namespace TiendaServicios.Api.Autor.Migrations
{
    public partial class MigracionPostgresFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutorLibrGuid",
                table: "AutorLibro");

            migrationBuilder.AddColumn<string>(
                name: "AutorLibroGuid",
                table: "AutorLibro",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutorLibroGuid",
                table: "AutorLibro");

            migrationBuilder.AddColumn<string>(
                name: "AutorLibrGuid",
                table: "AutorLibro",
                type: "text",
                nullable: true);
        }
    }
}
