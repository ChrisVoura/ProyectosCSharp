using FleetTrackAPI.Models;


namespace FleetTrackAPI.Respositories
{
    public interface IPedidoRepository
    {
        Task<List<Pedido>> ObtenerTodos();
        Task<List<Pedido>> ObtenerPendientes();
        Task<Pedido?> ObtenerPorId(int id);
        Task<Pedido> Crear(Pedido pedido);
        Task<Pedido> Editar(int id, string estado);
        Task<bool> Eliminar(int id);
    }
}