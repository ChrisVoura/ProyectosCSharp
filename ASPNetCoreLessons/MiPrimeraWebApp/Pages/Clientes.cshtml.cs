using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MiPrimeraWebApp.Data;


namespace MiPrimeraWebApp.Pages;

public class ClientesModel : PageModel
{
    [BindProperty] 
    public Cliente? NuevoCliente { get; set; } 

    private readonly AppDbContext _db;

    public List<Cliente> Clientes {get; set;}

    public ClientesModel(AppDbContext db)
    {
        _db = db;
        Clientes = new List<Cliente>();
    }

    public string? EmailPrefllenado { get; set; }

    public async Task OnGetAsync(string? email)
    {
        //var usuarioLogueado = HttpContext.Session.GetString("UsuarioId");
        
        //if (string.IsNullOrEmpty(usuarioLogueado))
        //{
          //  Response.Redirect("/Login");
          //  return;
        //}

        Clientes = await _db.Clientes.AsNoTracking().ToListAsync();
        Clientes = Clientes.OrderBy(c => c.Name).ToList();
    }

    public async Task<IActionResult> OnPostDelete(int id)
    {
        var cliente = await _db.Clientes.FindAsync(id);

        if (cliente is not null)
        {
            _db.Clientes.Remove(cliente);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Cliente '{cliente.Name}' eliminado exitosamente.";
        }

        return RedirectToPage("/Clientes");
    }
}