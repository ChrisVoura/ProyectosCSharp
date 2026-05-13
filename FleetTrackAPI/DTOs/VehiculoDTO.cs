using System.ComponentModel.DataAnnotations;


namespace FleetTrackAPI.DTOs
{
    public class RespuestaVehiculoDTO
    {
        public int Id {get; set;}
        public string? Placa {get; set;}
        public string? Tipo {get; set;}
        public double Capacidad {get; set;}
        public string? Estado {get; set;}
    }

    public class CrearVehiculoDTO
    {
        [Required]
        public string? Placa {get; set;}

        [Required]
        public string? Tipo {get; set;}

        [Range(1, 50000)]
        public double Capacidad {get; set;}
    }

    public class EditarVehiculoDTO
    {
        [Required]
        public string? Tipo {get; set;}

        [Range(1, 50000)]
        public double Capacidad {get; set;}

        [Required]
        public string? Estado {get; set;}
    }
}