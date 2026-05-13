using Microsoft.EntityFrameworkCore;
using FleetTrackAPI.Models;
using FleetTrackAPI.Data;


namespace FleetTrackAPI.Respositories
{
    public class EntregaRepository : IEntregaRepository
    {
        private readonly AppDbContext _context;
        public EntregaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Entrega>> ObtenerTodos()
        {
            return await _context.Entregas
            .Include(e => e.Pedido)
            .Include(e => e.Conductor)
            .Include(e => e.Vehiculo)
            .ToListAsync();
        }

        public async Task<List<Entrega>> ObtenerRuta()
        {
            return await _context.Entregas
            .Include(e => e.Pedido)
            .Include(e => e.Conductor)
            .Include(e => e.Vehiculo)
            .Where(e => e.Estado == "EnRuta")
            .ToListAsync();
        }

        public async Task<Entrega?> ObtenerPorId(int id)
        {
            return await _context.Entregas
                .Include(e => e.Pedido)
                .Include(e => e.Conductor)
                .Include(e => e.Vehiculo)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Entrega> Crear(Entrega entrega)
        {
            entrega.Estado = "EnRuta";
            entrega.FechaAsignacion = DateTime.Now;
            _context.Entregas.Add(entrega);
            await _context.SaveChangesAsync();
            return entrega;
        }

        public async Task<Entrega> Completar(int id, string observaciones)
        {
            var entrega = await ObtenerPorId(id);
            if (entrega == null) return null;

            entrega.Estado = "Completada";
            entrega.FechaEntrega = DateTime.Now;
            entrega.Observaciones = observaciones;

            await _context.SaveChangesAsync();
            return entrega;   
        }

    }
}