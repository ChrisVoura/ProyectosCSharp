using FleetTrackAPI.Models;

namespace FleetTrackAPI.Respositories
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> ObtenerTodos();
        Task<Cliente?> ObtenerPorId(int id);
        Task<Cliente> Crear(Cliente cliente);
        Task<Cliente> Editar(int id, Cliente cliente);
        Task<bool> Eliminar(int id);
    }
}