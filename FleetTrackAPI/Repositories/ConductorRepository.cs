using Microsoft.EntityFrameworkCore;
using FleetTrackAPI.Models;
using FleetTrackAPI.Data;


namespace FleetTrackAPI.Respositories
{
    public class ConductorRepository : IConductorRepository
    {
        private readonly AppDbContext _context;
        public ConductorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Conductor>> ObtenerTodos()
        {
            return await _context.Conductores.ToListAsync();
        }

        public async Task<Conductor?> ObtenerPorId(int id)
        {
            return await _context.Conductores
            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Conductor> Crear(Conductor conductor)
        {
            conductor.Estado = "Disponible";
            _context.Conductores.Add(conductor);
            await _context.SaveChangesAsync();
            return conductor;
        }

        public async Task<Conductor> Editar(int id, Conductor conductorEditado)
        {
            var conductor = await ObtenerPorId(id);
            if (conductor == null) return null;

            conductor.Nombre = conductorEditado.Nombre;
            conductor.Telefono = conductorEditado.Telefono;
            conductor.Estado = conductorEditado.Estado;

            await _context.SaveChangesAsync();
            return conductor;   
        }

        public async Task<bool> Eliminar(int id)
        {
            var conductor = await ObtenerPorId(id);
            if (conductor == null) return false;

            _context.Conductores.Remove(conductor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}