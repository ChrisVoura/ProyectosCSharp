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
    public class ConductoresController : ControllerBase
    {
        private readonly IConductorRepository  _repo;


        public ConductoresController(IConductorRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var coductores = await _repo.ObtenerTodos();
            var dtos = coductores.Select(c => new RespuestaConductorDTO
            {
                Id       = c.Id,
                Nombre   = c.Nombre,
                Licencia = c.Licencia,
                Telefono = c.Telefono,
                Estado   = c.Estado
                
            }).ToList();

            return Ok(dtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var conductor = await _repo.ObtenerPorId(id);
            if(conductor == null)
            {
                return NotFound($"Conductor {id} no encontrado");
            }

            return Ok(new RespuestaConductorDTO
            {
                Id       = conductor.Id,
                Nombre   = conductor.Nombre,
                Licencia = conductor.Licencia,
                Telefono = conductor.Telefono,
                Estado   = conductor.Estado            
            });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearConductorDTO dto)
        {
            var conductor = new Conductor
            {
                Nombre   = dto.Nombre,
                Licencia = dto.Licencia,
                Telefono = dto.Telefono

            };

            var creado = await _repo.Crear(conductor);
            return CreatedAtAction(nameof(GetPorId), new {id = creado.Id},
            new RespuestaConductorDTO
            {
                    Id       = creado.Id,
                    Nombre   = creado.Nombre,
                    Licencia = creado.Licencia,
                    Telefono = creado.Telefono,
                    Estado   = creado.Estado
            });

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] EditarConductorDTO dto)
        {
            var conductor = new Conductor
            {
                Nombre   = dto.Nombre,
                Telefono = dto.Telefono,
                Estado   = dto.Estado
            };

            var editado = await _repo.Editar(id,conductor);
            if(editado == null)
            {
                return NotFound($"Conductor {id} no encontrado.");
            }
            return Ok(new RespuestaConductorDTO
            {
                Id       = editado.Id,
                Nombre   = editado.Nombre,
                Licencia = editado.Licencia,
                Telefono = editado.Telefono,
                Estado   = editado.Estado                
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool eliminado = await _repo.Eliminar(id);
            if (!eliminado)
            {
                return NotFound($"Conductor {id} no encontrado.");
            }    
            return NoContent();
        }
    }
}