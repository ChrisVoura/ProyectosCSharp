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

        [BindProperty]
        public string? ImageUrl1 { get; set; }

        [BindProperty]
        public string? ImageUrl2 { get; set; }

        [BindProperty]
        public string? ImageUrl3 { get; set; }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string? ImageUrls { get; set; }

        public List<Producto> Productos { get; set; }

        public ProductosModel(AppDbContext db)
        {
            _db = db;
            Productos = new List<Producto>();
        }
        public void OnGet()
        {
            Productos = _db.Productos.ToList();

            if (Productos.Count == 0)
            {
                ModelState.AddModelError("Productos", "No hay productos disponibles.");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Combinar las 3 URLs en un string separado por comas
            var imageUrls = new List<string>();
            if (!string.IsNullOrWhiteSpace(ImageUrl1)) imageUrls.Add(ImageUrl1);
            if (!string.IsNullOrWhiteSpace(ImageUrl2)) imageUrls.Add(ImageUrl2);
            if (!string.IsNullOrWhiteSpace(ImageUrl3)) imageUrls.Add(ImageUrl3);
            var combinedImageUrl = string.Join(",", imageUrls);

            var nuevoProducto = new Producto
            {
                Name = Nombre ?? string.Empty,
                Price = Precio,
                Description = Descripcion ?? string.Empty,
                Category = Categoria ?? string.Empty,
                ImageUrl = combinedImageUrl
            };
            _db.Productos.Add(nuevoProducto);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Productos");
        }

        public IActionResult OnPostDelete(int id)
        {
            var producto = _db.Productos.Find(id);
            if (producto != null)
            {
                _db.Productos.Remove(producto);
                _db.SaveChanges();
            }
            return RedirectToPage("/Productos");
        }

        public IActionResult OnPostEdit()
        {
            var producto = _db.Productos.Find(Id);
            if (producto != null)
            {
                producto.Name = Nombre ?? string.Empty;
                producto.Price = Precio;
                producto.Description = Descripcion ?? string.Empty;
                producto.Category = Categoria ?? string.Empty;
                
                var imageUrls = new List<string>();
                if (!string.IsNullOrWhiteSpace(ImageUrl1)) imageUrls.Add(ImageUrl1);
                if (!string.IsNullOrWhiteSpace(ImageUrl2)) imageUrls.Add(ImageUrl2);
                if (!string.IsNullOrWhiteSpace(ImageUrl3)) imageUrls.Add(ImageUrl3);
                producto.ImageUrl = string.Join(",", imageUrls);
                
                _db.SaveChanges();
            }
            return RedirectToPage("/Productos");
        }
    }
}