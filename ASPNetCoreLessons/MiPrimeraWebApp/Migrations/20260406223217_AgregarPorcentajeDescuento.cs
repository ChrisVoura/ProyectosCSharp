using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiPrimeraWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPorcentajeDescuento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PorcentajeDescuento",
                table: "Productos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PorcentajeDescuento",
                table: "Productos");
        }
    }
}
