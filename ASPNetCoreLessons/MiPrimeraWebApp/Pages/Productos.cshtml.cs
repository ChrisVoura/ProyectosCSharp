using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiPrimeraWebApp.Data;

namespace MiPrimeraWebApp.Pages
{
    public class ProductosModel : PageModel
    {
        private readonly AppDbContext _db;
        [BindProperty]
        public string? Nombre { get; set; }

        [BindProperty]
        public decimal Precio { get; set; }

        [BindProperty]
        public string? Descripcion { get; set; }

        [BindProperty]
        public string? Categoria { get; set; }

        public ProductosModel(AppDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var nuevoProducto = new Producto
            {
                Name = Nombre ?? string.Empty,
                Price = Precio,
                Description = Descripcion ?? string.Empty,
                Category = Categoria ?? string.Empty
            };
            _db.Productos.Add(nuevoProducto);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Productos");
        }
    }
}