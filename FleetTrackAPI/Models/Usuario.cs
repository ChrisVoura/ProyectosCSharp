using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FleetTrackAPI.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}
        public string? Nombre {get; set;}
        public string? Email {get;set;}
        public string? Password {get; set;}
        public string? Rol {get; set;}
        
    }
}