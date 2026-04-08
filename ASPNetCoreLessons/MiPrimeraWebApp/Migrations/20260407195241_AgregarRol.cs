using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiPrimeraWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Clientes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Clientes");
        }
    }
}
