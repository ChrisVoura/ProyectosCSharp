using Microsoft.EntityFrameworkCore;
using FleetTrackAPI.Models;
using FleetTrackAPI.Data;


namespace FleetTrackAPI.Respositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly AppDbContext _context;
        public VehiculoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vehiculo>> ObtenerTodos()
        {
            return await _context.Vehiculos.ToListAsync();
        }

        public async Task<List<Vehiculo>> ObtenerDisponibles()
        {
            return await _context.Vehiculos
            .Where(v => v.Estado == "Disponible")
            .ToListAsync();
        }

        public async Task<Vehiculo?> ObtenerPorId(int id)
        {
            return await _context.Vehiculos
            .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehiculo> Crear(Vehiculo vehiculo)
        {
            vehiculo.Estado = "Disponible";
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
            return vehiculo;
        }

        public async Task<Vehiculo> Editar(int id, Vehiculo vehiculoEditado)
        {
            var vehiculo = await ObtenerPorId(id);
            if (vehiculo == null) return null;

            vehiculo.Tipo = vehiculoEditado.Tipo;
            vehiculo.Capacidad = vehiculoEditado.Capacidad;
            vehiculo.Estado = vehiculoEditado.Estado;

            await _context.SaveChangesAsync();
            return vehiculo;   
        }

        public async Task<bool> Eliminar(int id)
        {
            var vehiculo = await ObtenerPorId(id);
            if (vehiculo == null) return false;

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}