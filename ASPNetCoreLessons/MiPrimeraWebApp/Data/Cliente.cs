using System.ComponentModel.DataAnnotations;

namespace MiPrimeraWebApp.Data;

public  class Cliente
{
    public int Id {get ; set; }
  
    required public string Name {get; set; }

    required public string Apellido {get; set; }
    [StringLength(100)] 
    [EmailAddress]
    required public string Email {get; set; }

    required public string Password {get; set; }
    required public DateTime FechaRegistro {get; set; } = DateTime.Now;
}