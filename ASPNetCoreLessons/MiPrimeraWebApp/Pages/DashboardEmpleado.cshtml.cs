using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiPrimeraWebApp.Data;

namespace MiPrimeraWebApp.Pages
{
    public class DashboardEmpleadoModel : PageModel
    {
        private readonly AppDbContext _db;

        public int ProductosStockBajo { get; set; }
        public int PedidosRecientes { get; set; }
        public List<Producto> ProductosStockBajoLista { get; set; } = new();
        public List<Pedido> PedidosRecientesLista { get; set; } = new();
        public string UsuarioRol { get; set; } = "";

        public DashboardEmpleadoModel(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult OnGet()
        {
            UsuarioRol = HttpContext.Session.GetString("UsuarioRol") ?? "";
            
            if (UsuarioRol != "Empleado" && UsuarioRol != "Administrador")
            {
                return RedirectToPage("/Index");
            }
            
            var fechaLimite = DateTime.Now.AddDays(-7);
            
            ProductosStockBajo = _db.Productos.Count(p => p.Stock < 10);
            PedidosRecientes = _db.Pedidos.Count(p => p.FechaPedido >= fechaLimite);
            
            ProductosStockBajoLista = _db.Productos
                .Where(p => p.Stock < 10)
                .OrderBy(p => p.Stock)
                .ToList();
            
            PedidosRecientesLista = _db.Pedidos
                .Where(p => p.FechaPedido >= fechaLimite)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();
            
            return Page();
        }
    }
}
