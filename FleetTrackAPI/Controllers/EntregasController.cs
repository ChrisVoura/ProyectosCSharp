using Microsoft.AspNetCore.Mvc;
using FleetTrackAPI.Respositories;
using FleetTrackAPI.DTOs;
using FleetTrackAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.Serialization;

namespace FleetTrackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EntregasController : ControllerBase
    {
        private readonly IEntregaRepository  _repo;
        private readonly IPedidoRepository _repoPedido;
        private readonly IConductorRepository _repoConductor;
        private readonly IVehiculoRepository _repoVehiculo;


        public EntregasController(
            IEntregaRepository repo,
            IPedidoRepository repoPedido,
            IConductorRepository repoConductor,
            IVehiculoRepository repoVehiculo)
        {
            _repo = repo;
            _repoPedido = repoPedido;
            _repoConductor = repoConductor;
            _repoVehiculo = repoVehiculo;
        }


        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var entregas = await _repo.ObtenerTodos();
            return Ok(MapearDtos(entregas));
        }


        [HttpGet("en-ruta")]
        public async Task<IActionResult> GetEnRuta()
        {
            var entregas = await _repo.ObtenerRuta();
            return Ok(MapearDtos(entregas));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(int id)
        {
            var entrega = await _repo.ObtenerPorId(id);
            if(entrega == null)
            {
                return NotFound($"Entrega {id} no encontrado");
            }

            return Ok(MapearDto(entrega));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEntregaDTO dto)
        {
            //Validaciones de Pedido, Conductor y Vehiculo
           var pedido = await _repoPedido.ObtenerPorId(dto.PedidoId);
           if(pedido == null)
            {
                return NotFound($"Pedido {dto.PedidoId} no encontrado");
            }
                
           if(pedido.Estado != "Pendiente")
            {
                return BadRequest("El pedido no esté en estado Pendiente");
            } 


            var conductor = await _repoConductor.ObtenerPorId(dto.ConductorId);
           if(conductor == null)
            {
                return NotFound($"Conductor {dto.ConductorId} no encontrado");
            }
                
           if(conductor.Estado != "Disponible")
            {
                return BadRequest("El conductor no esta disponible");
            }     


            var vehiculo = await _repoVehiculo.ObtenerPorId(dto.VehiculoId);
           if(vehiculo == null)
            {
                return NotFound($"Vehiculo {dto.VehiculoId} no encontrado");
            }
                
           if(conductor.Estado != "Disponible")
            {
                return BadRequest("El vehiculo no esta disponible");
            }     

           
            var entrega = new Entrega
            {
                PedidoId      = dto.PedidoId,
                ConductorId   = dto.ConductorId,
                VehiculoId    = dto.VehiculoId,
                Observaciones = dto.Observaciones
            };

            var creado = await _repo.Crear(entrega);
            //Actualizar los estados
            await _repoPedido.Editar(dto.PedidoId, "EnRuta");
            conductor.Estado = "EnRuta";
            vehiculo.Estado = "EnRuta";
            await _repoConductor.Editar(dto.ConductorId, conductor);
            await _repoVehiculo.Editar(dto.VehiculoId, vehiculo);
            
            var entregaCompleta = await _repo.ObtenerPorId(creado.Id);
            return CreatedAtAction(nameof(GetPorId), new {id = creado.Id},
            MapearDto(entregaCompleta));
        }

        [HttpPut("{id}/completar")]
        public async Task<IActionResult> Completar(int id, [FromBody] EntregaCompletaDTO dto)
        {

            var entrega = await _repo.ObtenerPorId(id);
            if(entrega == null)
            {
                return NotFound($"Entrega {id} no encontrado.");
            }
            if(entrega.Estado == "Completa")
            {
                return BadRequest("La entrega ya fue completada.");
            }

            var completada = await _repo.Completar(id, dto.Observaciones);

            //Liberar coductor y vehiculo
            var conductor = await _repoConductor.ObtenerPorId(entrega.ConductorId);
            var vehiculo = await _repoVehiculo.ObtenerPorId(entrega.VehiculoId);

            conductor.Estado = "Disponible";
            vehiculo.Estado = "Disponible";
            await _repoConductor.Editar(entrega.ConductorId , conductor);
            await _repoVehiculo.Editar(entrega.VehiculoId, vehiculo);

            await _repoPedido.Editar(entrega.PedidoId, "Entregado");


            return Ok(MapearDto(completada));
        }

        private List<RespuestaEntregaDTO> MapearDtos(List<Entrega?> entregas) =>
            entregas.Select(e => MapearDto(e)).ToList();
        private RespuestaEntregaDTO MapearDto(Entrega? e)
        {
            return new RespuestaEntregaDTO
            {
            Id              = e.Id,
            Estado          = e.Estado,
            Observaciones   = e.Observaciones,
            FechaAsignacion = e.FechaAsignacion,
            FechaEntrega    = e.FechaEntrega,
            PedidoOrigen    = e.Pedido?.Origen,
            PedidoDestino   = e.Pedido?.Destino,
            ConductorNombre = e.Conductor?.Nombre,
            VehiculoPlaca   = e.Vehiculo?.Placa                
            };
        }
    }
}