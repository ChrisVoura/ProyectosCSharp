using FleetTrackAPI.Models;

namespace FleetTrackAPI.Respositories
{
    public interface IEntregaRepository
    {
        Task<List<Entrega>> ObtenerTodos();
        Task<List<Entrega>> ObtenerRuta();
        Task<Entrega?> ObtenerPorId(int id);
        Task<Entrega> Crear(Entrega entrega);
        Task<Entrega> Completar(int id, string observaciones);
    }
}