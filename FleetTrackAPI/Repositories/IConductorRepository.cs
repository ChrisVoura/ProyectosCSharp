using FleetTrackAPI.Models;

namespace FleetTrackAPI.Respositories
{
    public interface IConductorRepository
    {
        Task<List<Conductor>> ObtenerTodos();
        Task<Conductor?> ObtenerPorId(int id);
        Task<Conductor> Crear(Conductor conductor);
        Task<Conductor> Editar(int id, Conductor conductor);
        Task<bool> Eliminar(int id);
    }
}