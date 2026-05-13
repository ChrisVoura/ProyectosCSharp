using System.ComponentModel.DataAnnotations;


namespace FleetTrackAPI.DTOs
{
    public class RespuestaEntregaDTO
    {
        public int Id {get; set;}
        public string? Estado {get; set;}
        public string? Observaciones {get; set;}
        public DateTime FechaAsignacion {get; set;}
        public DateTime? FechaEntrega {get; set;}

        //Esenciales referencias para mostrar en la respuesta
        public string? PedidoOrigen {get; set;}
        public string? PedidoDestino {get; set;}
        public string? ConductorNombre {get; set;}
        public string? VehiculoPlaca {get; set;}
    }

    public class CrearEntregaDTO
    {
        [Required]
        public int PedidoId {get; set;}

        [Required]
        public int ConductorId {get; set;}

        [Required]
        public int VehiculoId {get; set;}

        public string? Observaciones {get; set;}
    }

    public class EntregaCompletaDTO
    {
        public string? Observaciones {get; set;}
    }
}