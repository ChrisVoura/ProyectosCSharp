using System.ComponentModel.DataAnnotations;


namespace FleetTrackAPI.DTOs
{
    public class RespuestaClienteDTO
    {
        public int Id {get; set;}
        public string? Nombre {get; set;}
        public string? Email {get; set;}
        public string? Telefono {get; set;}
        public string? Direccion {get; set;}
    }

    public class CrearClienteDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? Nombre {get; set;}

        [Required]
        [EmailAddress]
        public string? Email {get; set;}

        [Phone]
        public string? Telefono {get; set;}

        [Required]
        public string? Direccion {get; set;}
    }

    public class EditarClienteDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? Nombre {get; set;}

        [EmailAddress]
        public string? Email {get; set;}

        [Phone]
        public string? Telefono {get; set;}

        [Required]
        public string? Direccion {get; set;}
    }
}