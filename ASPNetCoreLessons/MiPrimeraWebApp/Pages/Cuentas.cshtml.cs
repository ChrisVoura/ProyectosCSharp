using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiPrimeraWebApp.Data;


namespace MiPrimeraWebApp.Pages
{
    public class CuentasModel : PageModel
    {
        private readonly AppDbContext _db;

        public CuentasModel(AppDbContext db)
        {
            _db = db;
        }
        
        public string? NombreUsuario { get; set; }

        public void OnGet()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
            {
                Response.Redirect("/Login");
                return;
            }
            
            NombreUsuario = HttpContext.Session.GetString("UsuarioNombre");
        }
    }
}
