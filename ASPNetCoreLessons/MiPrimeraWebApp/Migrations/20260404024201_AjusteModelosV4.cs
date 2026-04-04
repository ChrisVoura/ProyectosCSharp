using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiPrimeraWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AjusteModelosV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Clientes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Clientes");
        }
    }
}
