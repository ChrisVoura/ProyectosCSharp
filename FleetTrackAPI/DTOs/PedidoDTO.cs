using System.ComponentModel.DataAnnotations;


namespace FleetTrackAPI.DTOs
{
    public class RespuestaPedidoDTO
    {
        public int Id {get; set;}
        public string? Origen {get; set;}
        public string? Destino {get; set;}
        public double Peso {get; set;}
        public string? Descripcion {get; set;}
        public string? Estado {get; set;}
        public DateTime FechaCreacion {get; set;}
        public string? ClienteNombre {get; set;}

    }

    public class CrearPedidoDTO
    {
        [Required]
        public string? Origen {get; set;}

        [Required]
        public string? Destino {get; set;}

        [Range(0.1, 50000)]
        public double Peso {get; set;}

        public string? Descripcion {get; set;}

        [Required]
        public int ClienteId {get; set;}
    }

    public class EditarEstadoPedidoDTO
    {
        [Required]
        public string? Estado {get; set;}
    }

}