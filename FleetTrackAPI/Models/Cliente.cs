using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace FleetTrackAPI.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}
        public string? Nombre {get; set;}
        public string? Email {get;set;}
        public string? Telefono {get; set;}
        public string? Direccion {get; set;}



        public List<Pedido>? Pedidos {get; set;}
    }
}