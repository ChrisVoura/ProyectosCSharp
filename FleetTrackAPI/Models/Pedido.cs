using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FleetTrackAPI.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int Id {get; set;}
        public string? Origen {get; set;}
        public string? Destino {get; set;}
        public double Peso {get; set;}
        public string? Descripcion {get; set;}
        public string? Estado {get; set;}
        public DateTime FechaCreacion {get; set;}= DateTime.Now;


        public int ClienteId {get; set;}
        public Cliente? Cliente {get; set;} 
        public Entrega? Entrega {get; set;}
    }
}