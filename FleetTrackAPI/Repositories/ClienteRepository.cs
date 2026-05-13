using Microsoft.EntityFrameworkCore;
using FleetTrackAPI.Models;
using FleetTrackAPI.Data;


namespace FleetTrackAPI.Respositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;
        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> ObtenerTodos()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente?> ObtenerPorId(int id)
        {
            return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente> Crear(Cliente cliente)
        {
        
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> Editar(int id, Cliente clienteEditado)
        {
            var cliente = await ObtenerPorId(id);
            if (cliente == null) return null;

            cliente.Nombre = clienteEditado.Nombre;
            cliente.Email = clienteEditado.Email;
            cliente.Telefono = clienteEditado.Telefono;
            cliente.Direccion = clienteEditado.Direccion;

            await _context.SaveChangesAsync();
            return cliente;   
        }

        public async Task<bool> Eliminar(int id)
        {
            var cliente = await ObtenerPorId(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}