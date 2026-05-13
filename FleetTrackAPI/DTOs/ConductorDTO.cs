using System.ComponentModel.DataAnnotations;


namespace FleetTrackAPI.DTOs
{
    public class RespuestaConductorDTO
    {
        public int Id {get; set;}
        public string? Nombre {get; set;}
        public string? Licencia {get; set;}
        public string? Telefono {get; set;}
        public string? Estado {get; set;}
    }

    public class CrearConductorDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? Nombre {get; set;}

        [Required]
        public string? Licencia {get; set;}

        [Phone]
        public string? Telefono {get; set;}

    }

    public class EditarConductorDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? Nombre {get; set;}

        [Phone]
        public string? Telefono {get; set;}

        [Required]
        public string? Estado {get; set;}
    }
}