using Microsoft.AspNetCore.Mvc;
using FleetTrackAPI.Respositories;
using FleetTrackAPI.DTOs;
using FleetTrackAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace FleetTrackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository  _repo;


        public ClientesController(IClienteRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var clientes = await _repo.ObtenerTodos();
            var dtos = clientes.Select(c => new RespuestaClienteDTO
            {
                Id        = c.Id,
                Nombre    = c.Nombre,
                Email     = c.Email,
                Telefono  = c.Telefono,
                Direccion = c.Direccion
                
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var cliente = await _repo.ObtenerPorId(id);
            if(cliente == null)
            {
                return NotFound($"Cliente {id} no encontrado");
            }

            return Ok(new RespuestaClienteDTO
            {
                Id        = cliente.Id,
                Nombre    = cliente.Nombre,
                Email     = cliente.Email,
                Telefono  = cliente.Telefono,
                Direccion = cliente.Direccion                
            });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearClienteDTO dto)
        {
            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion
            };

            var creado = await _repo.Crear(cliente);
            return CreatedAtAction(nameof(GetPorId), new {id = creado.Id},
            new RespuestaClienteDTO
            {
                Id        = creado.Id,
                Nombre    = creado.Nombre,
                Email     = creado.Email,
                Telefono  = creado.Telefono,
                Direccion = creado.Direccion
            });

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] EditarClienteDTO dto)
        {
            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion
            };

            var editado = await _repo.Editar(id,cliente);
            if(editado == null)
            {
                return NotFound($"Cliente {id} no encontrado.");
            }
            return Ok(new RespuestaClienteDTO
            {
                Id        = editado.Id,
                Nombre    = editado.Nombre,
                Email     = editado.Email,
                Telefono  = editado.Telefono,
                Direccion = editado.Direccion                
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool eliminado = await _repo.Eliminar(id);
            if (!eliminado)
            {
                return NotFound($"Cliente {id} no encontrado.");
            }    
            return NoContent();
        }
    }
}