namespace FleetTrackAPI.Models
{
    public class Entrega
    {
        public int Id {get; set;}
        public string? Estado {get; set;}
        public string? Observaciones {get; set;}
        public DateTime FechaAsignacion {get; set;} = DateTime.Now;
        public DateTime? FechaEntrega {get; set;}
        




        public int PedidoId {get; set;}
        public Pedido? Pedido {get; set;}
        public int VehiculoId {get; set;}
        public Vehiculo? Vehiculo {get; set;}
        public int ConductorId {get; set;}
        public Conductor? Conductor {get; set;}
    }
}