using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTrackAPI.Migrations
{
    /// <inheritdoc />
public partial class FixFechaCreacion : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            ALTER TABLE ""Pedidos"" 
            ALTER COLUMN ""FechaCreacion"" TYPE timestamp with time zone 
            USING ""FechaCreacion""::timestamp with time zone;
        ");

        migrationBuilder.Sql(@"
            ALTER TABLE ""Entregas"" 
            ALTER COLUMN ""FechaAsignacion"" TYPE timestamp with time zone 
            USING ""FechaAsignacion""::timestamp with time zone;
        ");

        migrationBuilder.Sql(@"
            ALTER TABLE ""Entregas"" 
            ALTER COLUMN ""FechaEntrega"" TYPE timestamp with time zone 
            USING ""FechaEntrega""::timestamp with time zone;
        ");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
}
