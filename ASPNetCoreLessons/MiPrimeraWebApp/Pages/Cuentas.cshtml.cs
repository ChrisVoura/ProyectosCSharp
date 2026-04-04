using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiPrimeraWebApp.Data;


namespace MiPrimeraWebApp.Pages
{
    public class CuentasModel : PageModel
    {
        private readonly AppDbContext _db;

        public List<Pedido>? Pedidos { get; set; }
        public List<string>? Direcciones { get; set; }

        public CuentasModel(AppDbContext db)
        {
            _db = db;
        }
        
        [BindProperty]
        public string? NombreUsuario { get; set; }
        [BindProperty]
        public string? Nombre { get; set; }
        [BindProperty]
        public string? Apellido { get; set; }
        [BindProperty]
        public string? Email { get; set; }
        [BindProperty]
        public string? Genero { get; set; }
        [BindProperty]
        public string? Telefono { get; set; }
        [BindProperty]
        public string? Direccion { get; set; }
        [BindProperty]
        public string? NuevaDireccion { get; set; }
        [BindProperty]
        public DateTime? FechaNacimiento { get; set; }

        public void OnGet()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
            {
                Response.Redirect("/Login");
                return;
            }
            
            var id = int.Parse(usuarioId);
            var cliente = _db.Clientes.Find(id);
            
            if (cliente == null)
            {
                Response.Redirect("/Login");
                return;
            }
            
            NombreUsuario = $"{cliente.Name} {cliente.Apellido}";
            Nombre = cliente.Name;
            Apellido = cliente.Apellido;
            Email = cliente.Email;
            Genero = cliente.Genero;
            Telefono = cliente.Telefono;
            FechaNacimiento = cliente.fechaNacimiento;
            Direcciones = string.IsNullOrEmpty(cliente.Direccion) 
                ? new List<string>() 
                : new List<string> { cliente.Direccion };
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Home");
        }

        public IActionResult OnPostActualizar()
        {
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Apellido: {Apellido}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Telefono: {Telefono}");
            Console.WriteLine($"Genero: {Genero}");
            Console.WriteLine($"FechaNacimiento: {FechaNacimiento}");
            
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
            {
                Response.Redirect("/Login");
                return Page();
            }
            
            var id = int.Parse(usuarioId);
            var cliente = _db.Clientes.Find(id);
            
            if (cliente == null)
            {
                Response.Redirect("/Login");
                return Page();
            }
            
            
            if (!string.IsNullOrEmpty(Nombre)) cliente.Name = Nombre;
            if (!string.IsNullOrEmpty(Apellido)) cliente.Apellido = Apellido;
            if (!string.IsNullOrEmpty(Email)) cliente.Email = Email;
            if (!string.IsNullOrEmpty(Telefono)) cliente.Telefono = Telefono;
            if (FechaNacimiento.HasValue) cliente.fechaNacimiento = FechaNacimiento;
            if (!string.IsNullOrEmpty(Genero)) cliente.Genero = Genero;
            
            
            _db.Clientes.Attach(cliente);
            var entry = _db.Entry(cliente);
            entry.Property(c => c.Name).IsModified = true;
            entry.Property(c => c.Apellido).IsModified = true;
            entry.Property(c => c.Email).IsModified = true;
            entry.Property(c => c.Telefono).IsModified = true;
            entry.Property(c => c.fechaNacimiento).IsModified = true;
            entry.Property(c => c.Genero).IsModified = true;
            _db.SaveChanges();
            
            return RedirectToPage("/Cuentas");
        }

        public IActionResult OnPostAgregarDireccion()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
            {
                Response.Redirect("/Login");
                return Page();
            }
            
            var id = int.Parse(usuarioId);
            var cliente = _db.Clientes.Find(id);
            
            if (cliente == null)
            {
                Response.Redirect("/Login");
                return Page();
            }
            
            if (!string.IsNullOrEmpty(NuevaDireccion))
            {
                cliente.Direccion = NuevaDireccion;
                _db.Clientes.Attach(cliente);
                var entry = _db.Entry(cliente);
                entry.Property(c => c.Direccion).IsModified = true;
                _db.SaveChanges();
            }
            
            return RedirectToPage("/Cuentas");
        }

        public IActionResult OnPostEliminarDireccion()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
            {
                Response.Redirect("/Login");
                return Page();
            }
            
            var id = int.Parse(usuarioId);
            var cliente = _db.Clientes.Find(id);
            
            if (cliente == null)
            {
                Response.Redirect("/Login");
                return Page();
            }
            
            if (!string.IsNullOrEmpty(Direccion))
            {
                cliente.Direccion = null;
                _db.Clientes.Attach(cliente);
                var entry = _db.Entry(cliente);
                entry.Property(c => c.Direccion).IsModified = true;
                _db.SaveChanges();
            }
            
            return RedirectToPage("/Cuentas");
        }
    }
}