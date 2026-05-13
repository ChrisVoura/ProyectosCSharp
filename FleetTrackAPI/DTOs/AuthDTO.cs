using System.ComponentModel.DataAnnotations;


namespace FleetTrackAPI.DTOs
{
    public class RegistroDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? Nombre {get; set;}

        [Required]
        [EmailAddress]
        public string? Email {get; set;}

        [Required]
        [MaxLength (12)]
        public string? Password {get; set;}

        [Required]
        public string? Rol {get; set;}

    }

    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string? Email {get; set;}

        [Required]
        public string? Password {get; set;}
    }

    public class RespuestaTokenDTO
    {
        public string? Token {get; set;}
        public string? Nombre {get; set;}
        public string? Email {get; set;}
        public string? Rol {get; set;}
    }
}