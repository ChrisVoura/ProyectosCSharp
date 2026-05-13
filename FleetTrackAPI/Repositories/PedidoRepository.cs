using Microsoft.EntityFrameworkCore;
using FleetTrackAPI.Models;
using FleetTrackAPI.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using FleetTrackAPI.DTOs;


namespace FleetTrackAPI.Respositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;
        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> ObtenerTodos()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public async Task<List<Pedido>> ObtenerPendientes()
        {
            return await _context.Pedidos
            .Include(p => p.Cliente)
            .Where(p => p.Estado == "Pendiente")
            .ToListAsync();
        }

        public async Task<Pedido?> ObtenerPorId(int id)
        {
            return await _context.Pedidos
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pedido> Crear(Pedido pedido)
        {
            pedido.Estado = "Pendiente";
            pedido.FechaCreacion = DateTime.Now;
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> Editar(int id, string estado)
        {
            var pedido = await ObtenerPorId(id);
            if (pedido == null) return null;

            pedido.Estado = estado;
            await _context.SaveChangesAsync();
            return pedido;
        }
            
        

        public async Task<bool> Eliminar(int id)
        {
            var pedido = await ObtenerPorId(id);
            if (pedido == null) return false;

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}