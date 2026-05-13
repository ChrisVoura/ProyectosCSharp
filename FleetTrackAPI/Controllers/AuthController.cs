using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetTrackAPI.Data;
using FleetTrackAPI.DTOs;
using FleetTrackAPI.Models;
using FleetTrackAPI.Services;

namespace FleetTrackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext  _context;
        private readonly JwtService  _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroDTO dto)
        {
            bool emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email == dto.Email);

            if (emailExiste)
            {
                return BadRequest("El email ya esta registrado.");
            }
            
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Rol = dto.Rol
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Usuario registrado exitosamente"});

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email);
        
            if(usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password))
            {
                return Unauthorized("Credenciales inválidas");
            }

            var token = _jwtService.GenerarToken(usuario);

            return Ok(new RespuestaTokenDTO
            {
               Token = token,
               Nombre = usuario.Nombre,
               Email = usuario.Email,
               Rol = usuario.Rol 
            });
        }
    }
}