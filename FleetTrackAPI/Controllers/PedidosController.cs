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
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoRepository  _repo;
        private readonly IClienteRepository _repoCliente;


        public PedidosController(IPedidoRepository repo, IClienteRepository repoCliente)
        {
            _repo = repo;
            _repoCliente = repoCliente;
        }


        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var pedidos = await _repo.ObtenerTodos();
            var dtos = pedidos.Select(p => new RespuestaPedidoDTO
            {
                Id            = p.Id,
                Origen        = p.Origen,
                Destino       = p.Destino,
                Peso          = p.Peso,
                Descripcion   = p.Descripcion,
                Estado        = p.Estado,
                FechaCreacion = p.FechaCreacion,
                ClienteNombre = p.Cliente?.Nombre
                
            }).ToList();

            return Ok(dtos);
        }


        [HttpGet("pendientes")]
        public async Task<IActionResult> GetPendientes()
        {
            var vehiculos = await _repo.ObtenerPendientes();
            var dtos = vehiculos.Select(p => new RespuestaPedidoDTO
            {
                Id            = p.Id,
                Origen        = p.Origen,
                Destino       = p.Destino,
                Peso          = p.Peso,
                Descripcion   = p.Descripcion,
                Estado        = p.Estado,
                FechaCreacion = p.FechaCreacion,
                ClienteNombre = p.Cliente?.Nombre           

            }).ToList();

            return Ok(dtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var pedido = await _repo.ObtenerPorId(id);
            if(pedido == null)
            {
                return NotFound($"Pedido {id} no encontrado");
            }

            return Ok(new RespuestaPedidoDTO
            {
                Id            = pedido.Id,
                Origen        = pedido.Origen,
                Destino       = pedido.Destino,
                Peso          = pedido.Peso,
                Descripcion   = pedido.Descripcion,
                Estado        = pedido.Estado,
                FechaCreacion = pedido.FechaCreacion,
                ClienteNombre = pedido.Cliente?.Nombre             
            });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearPedidoDTO dto)
        {
           var cliente = await _repoCliente.ObtenerPorId(dto.ClienteId);
           if(cliente == null)
            {
                return NotFound($"Cliente {dto.ClienteId} no encontrado");
            }
           
           
            var pedido = new Pedido
            {
                Origen      = dto.Origen,
                Destino     = dto.Destino,
                Peso        = dto.Peso,
                Descripcion = dto.Descripcion,
                ClienteId   = dto.ClienteId
            };

            var creado = await _repo.Crear(pedido);
            return CreatedAtAction(nameof(GetPorId), new {id = creado.Id},
            new RespuestaPedidoDTO
            {
                    Id            = creado.Id,
                    Origen        = creado.Origen,
                    Destino       = creado.Destino,
                    Peso          = creado.Peso,
                    Descripcion   = creado.Descripcion,
                    Estado        = creado.Estado,
                    FechaCreacion = creado.FechaCreacion,
                    ClienteNombre = cliente.Nombre
            });

        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> EditarEstado(int id, [FromBody] EditarEstadoPedidoDTO dto)
        {

            var editado = await _repo.Editar(id, dto.Estado);
            if(editado == null)
            {
                return NotFound($"Pedido {id} no encontrado.");
            }
            return Ok(new RespuestaPedidoDTO
            {
                Id            = editado.Id,
                Origen        = editado.Origen,
                Destino       = editado.Destino,
                Peso          = editado.Peso,
                Descripcion   = editado.Descripcion,
                Estado        = editado.Estado,
                FechaCreacion = editado.FechaCreacion,
                ClienteNombre = editado.Cliente?.Nombre              
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool eliminado = await _repo.Eliminar(id);
            if (!eliminado)
            {
                return NotFound($"Pedido {id} no encontrado.");
            }    
            return NoContent();
        }
    }
}