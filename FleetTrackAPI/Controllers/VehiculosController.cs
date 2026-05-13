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
    public class VehiculosController : ControllerBase
    {
        private readonly IVehiculoRepository  _repo;


        public VehiculosController(IVehiculoRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var vehiculos = await _repo.ObtenerTodos();
            var dtos = vehiculos.Select(v => new RespuestaVehiculoDTO
            {
                Id        = v.Id,
                Placa    = v.Placa,
                Tipo     = v.Tipo,
                Capacidad  = v.Capacidad,
                Estado = v.Estado
                
            }).ToList();

            return Ok(dtos);
        }


        [HttpGet("disponibles")]
        public async Task<IActionResult> GetDisponibles()
        {
            var vehiculos = await _repo.ObtenerDisponibles();
            var dtos = vehiculos.Select(v => new RespuestaVehiculoDTO
            {
                Id        = v.Id,
                Placa     = v.Placa,
                Tipo      = v.Tipo,
                Capacidad = v.Capacidad,
                Estado    = v.Estado               

            }).ToList();

            return Ok(dtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var vehiculo = await _repo.ObtenerPorId(id);
            if(vehiculo == null)
            {
                return NotFound($"Cliente {id} no encontrado");
            }

            return Ok(new RespuestaVehiculoDTO
            {
                Id        = vehiculo.Id,
                Placa     = vehiculo.Placa,
                Tipo      = vehiculo.Tipo,
                Capacidad = vehiculo.Capacidad,
                Estado    = vehiculo.Estado              
            });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearVehiculoDTO dto)
        {
            var vehiculo = new Vehiculo
            {
                Placa     = dto.Placa,
                Tipo      = dto.Tipo,
                Capacidad = dto.Capacidad
            };

            var creado = await _repo.Crear(vehiculo);
            return CreatedAtAction(nameof(GetPorId), new {id = creado.Id},
            new RespuestaVehiculoDTO
            {
                Id        = creado.Id,
                Placa     = creado.Placa,
                Tipo      = creado.Tipo,
                Capacidad = creado.Capacidad,
                Estado    = creado.Estado
            });

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] EditarVehiculoDTO dto)
        {
            var vehiculo = new Vehiculo
            {
                Tipo      = dto.Tipo,
                Capacidad = dto.Capacidad,
                Estado    = dto.Estado
            };

            var editado = await _repo.Editar(id,vehiculo);
            if(editado == null)
            {
                return NotFound($"Cliente {id} no encontrado.");
            }
            return Ok(new RespuestaVehiculoDTO
            {
                Id        = editado.Id,
                Placa     = editado.Placa,
                Tipo      = editado.Tipo,
                Capacidad = editado.Capacidad,
                Estado    = editado.Estado                
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool eliminado = await _repo.Eliminar(id);
            if (!eliminado)
            {
                return NotFound($"Vehículo {id} no encontrado.");
            }    
            return NoContent();
        }
    }
}