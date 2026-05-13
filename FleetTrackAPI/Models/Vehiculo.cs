using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetTrackAPI.Models
{
    public class Vehiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}
        public string? Placa {get; set;}
        public string? Tipo {get; set;}
        public double Capacidad {get; set;}
        public string? Estado {get; set;}



        public List<Entrega>? Entregas {get; set;}
    }
}