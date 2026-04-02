using Microsoft.AspNetCore.Mvc.RazorPages;
using MiPrimeraWebApp.Data;


namespace MiPrimeraWebApp.Pages
{
    public class DetallesModel : PageModel
    {
        private readonly AppDbContext _db;

        public Producto? Producto { get; set; }

        public DetallesModel(AppDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            if (Request.Query.ContainsKey("id") && int.TryParse(Request.Query["id"], out int id))
            {
                Producto = _db.Productos.FirstOrDefault(p => p.Id == id);
            }
        }
    }
}