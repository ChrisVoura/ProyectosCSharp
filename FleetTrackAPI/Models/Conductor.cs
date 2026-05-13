namespace FleetTrackAPI.Models
{
    public class Conductor
    {
        public int Id {get; set;}
        public string? Nombre {get; set;}
        public string? Licencia {get;set;}
        public string? Telefono {get; set;}
        public string? Estado {get; set;}

        
        
        public List<Entrega>? Entregas {get; set;}
    }
}