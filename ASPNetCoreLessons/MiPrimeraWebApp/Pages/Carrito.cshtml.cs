using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiPrimeraWebApp.Data;
using System.Text.Json;


namespace MiPrimeraWebApp.Pages
{
    public class CarritoModel : PageModel
    {
        private readonly AppDbContext _db;

        public List<Producto> ProductosEnCarrito { get; set; }
        public CarritoModel(AppDbContext db)
        {
            _db = db;
            ProductosEnCarrito = new List<Producto>();
        }

        private Dictionary<int, int> ObtenerCarrito()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            if (string.IsNullOrEmpty(carritoJson))
                return new Dictionary<int, int>();

            try
            {
                return JsonSerializer.Deserialize<Dictionary<int, int>>(carritoJson) ?? new Dictionary<int, int>();
            }
            catch
            {
                try
                {
                    var listaAntigua = JsonSerializer.Deserialize<List<int>>(carritoJson) ?? new List<int>();
                    var nuevo = listaAntigua.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
                    HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(nuevo));
                    return nuevo;
                }
                catch
                {
                    HttpContext.Session.Remove("Carrito");
                    return new Dictionary<int, int>();
                }
            }
        }

        private void GuardarCarrito(Dictionary<int, int> carrito)
        {
            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));
        }

        public void OnGet()
        {
            var carrito = ObtenerCarrito();
            var ids = carrito.Keys.ToList();
            ProductosEnCarrito = _db.Productos.Where(p => ids.Contains(p.Id)).ToList();
        }

        public IActionResult OnGetCarritoCount()
        {
            var carrito = ObtenerCarrito();
            return new JsonResult(new { count = carrito.Values.Sum() });
        }

        public IActionResult OnPostAgregar(int id)
        {
            var carrito = ObtenerCarrito();

            if (carrito.ContainsKey(id))
                carrito[id]++;
            else
                carrito[id] = 1;

            GuardarCarrito(carrito);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult OnGetContenido()
        {
            var carrito = ObtenerCarrito();
            var html = new System.Text.StringBuilder();

            if (carrito.Count == 0)
            {
                html.Append("<p class='text-center p-3'>Tu carrito está vacío</p>");
            }
            else
            {
                var ids = carrito.Keys.ToList();
                var productos = _db.Productos.Where(p => ids.Contains(p.Id)).ToList();
                var total = carrito.Sum(c => productos.First(p => p.Id == c.Key).Price * c.Value);
                var subtotal = total; 

                html.Append("<div class='p-3'>");
                foreach (var producto in productos)
                {
                    var cantidad = carrito[producto.Id];
                    var imageUrls = producto.ImageUrl?.Split(',').FirstOrDefault() ?? "";
                    html.Append($@"
                        <div class='card mb-2'>
                            <div class='card-body d-flex justify-content-between align-items-center'>
                                <div class='d-flex align-items-center'>
                                    <img src='{imageUrls}' alt='{producto.Name}' class='img-thumbnail me-3' style='width: 60px; height: 60px; object-fit: cover;'/>
                                    <div>
                                        <h6 class='card-title mb-1'>{producto.Name}</h6>
                                        <p class='card-text mb-0'>₡{producto.Price:F3}</p>
                                        <div class='d-flex align-items-center mt-1'>
                                            <button type='button' class='btn btn-sm btn-outline-secondary' onclick='cambiarCantidad({producto.Id}, -1)'>-</button>
                                            <span class='mx-2'>{cantidad}</span>
                                            <button type='button' class='btn btn-sm btn-outline-secondary' onclick='cambiarCantidad({producto.Id}, 1)'>+</button>
                                        </div>
                                    </div>
                                </div>
                                
                                <button type='button' class='btn p-0 border-0 bg-transparent' onclick='eliminarProducto({producto.Id})' id='carritoIcon'>
                                    <img src='https://img.icons8.com/ios/50/trash--v1.png' alt='trash--v1' width='25' height='25'/>
                                </button>
                            </div>
                        </div>");
                }
                html.Append("</div>");
                
                html.Append($@"
                    <div class='carrito-footer'>
                        <div class='d-flex justify-content-between align-items-center mb-2'>
                            <span class='fw-bold'>Subtotal:</span>
                            <span class='fw-bold fs-5'>₡{subtotal:F3}</span>
                            <span class='fw-bold'>Total:</span>
                            <span class='fw-bold fs-5'>₡{total:F3}</span>
                        </div>
                        <button class='btn btn-outline-danger w-100 mb-2' onclick='limpiarCarrito()'>Limpiar Carrito</button>
                        <button class='btn btn-success w-100' onclick='finalizarCompra()'>Finalizar Compra</button>
                    </div>");
            }

            return Content(html.ToString());
        }

        public IActionResult OnGetLimpiar()
        {
            HttpContext.Session.Remove("Carrito");
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetCambiarCantidad(int id, int cantidad)
        {
            var carrito = ObtenerCarrito();

            if (carrito.ContainsKey(id))
            {
                carrito[id] += cantidad;
                if (carrito[id] <= 0)
                    carrito.Remove(id);
            }

            GuardarCarrito(carrito);
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetEliminarProducto(int id)
        {
            var carrito = ObtenerCarrito();
            carrito.Remove(id);
            GuardarCarrito(carrito);
            return new JsonResult(new { success = true });
        }
    }
}
