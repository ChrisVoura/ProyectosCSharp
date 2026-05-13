using FleetTrackAPI.Models;

namespace FleetTrackAPI.Respositories
{
    public interface IVehiculoRepository
    {
        Task<List<Vehiculo>> ObtenerTodos();
        Task<List<Vehiculo>> ObtenerDisponibles();
        Task<Vehiculo?> ObtenerPorId(int id);
        Task<Vehiculo> Crear(Vehiculo vehiculo);
        Task<Vehiculo> Editar(int id, Vehiculo vehiculo);
        Task<bool> Eliminar(int id);
    }
}