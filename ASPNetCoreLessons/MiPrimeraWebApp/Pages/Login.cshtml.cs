using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiPrimeraWebApp.Data;


namespace MiPrimeraWebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _db;

        public LoginModel(AppDbContext db)
        {
            _db = db;
        }
        
        [BindProperty]
        public string Email { get; set; } = "";
        
        [BindProperty]
        public string Password { get; set; } = "";
        
        public string EmailVal { get; set; } = "";
        public bool ShowPassword { get; set; }
        
        public void OnGet()
        {
        }

        public IActionResult OnPostVerificar()
        {
            var emailIngresado = Request.Form["Email"].ToString();
            
            if (string.IsNullOrWhiteSpace(emailIngresado))
            {
                return Page();
            }
            
            var cliente = _db.Clientes
                .FirstOrDefault(c => c.Email != null && c.Email.ToLower() == emailIngresado.ToLower());
            
            if (cliente != null)
            {
                EmailVal = emailIngresado;
                ShowPassword = true;
                return Page();
            }
            else
            {
                return RedirectToPage("/Usuario", new { email = emailIngresado });
            }
        }

        public IActionResult OnPost()
        {
            var emailIngresado = Request.Form["Email"].ToString();
            var passwordIngresado = Request.Form["Password"].ToString();
            var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(passwordIngresado));
            
            var cliente = _db.Clientes.FirstOrDefault(c => c.Email != null && c.Email.ToLower() == emailIngresado.ToLower());
            
            if (cliente != null && cliente.Password == passwordHash)
            {
                HttpContext.Session.SetString("UsuarioId", cliente.Id.ToString());
                HttpContext.Session.SetString("UsuarioNombre", $"{cliente.Name} {cliente.Apellido}");
                return RedirectToPage("/Cuentas");
            }
            
            EmailVal = emailIngresado;
            ShowPassword = true;
            ViewData["ErrorMessage"] = "Email o contraseña incorrectos";
            return Page();
        }
    }
}
