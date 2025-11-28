using Api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de clientes.
    /// </summary>
    public class ClienteController : BaseController
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(IClienteService clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los clientes registrados.
        /// </summary>
        /// <returns>Lista de clientes.</returns>
        /// <response code="200">Retorna la lista de clientes.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
        {
            var clientes = await _clienteService.GetAllClientesAsync();
            return Ok(_mapper.Map<IEnumerable<ClienteDto>>(clientes));
        }

        /// <summary>
        /// Obtiene un cliente por su ID.
        /// </summary>
        /// <param name="id">El ID del cliente.</param>
        /// <returns>El cliente solicitado.</returns>
        /// <response code="200">Retorna el cliente.</response>
        /// <response code="404">Si el cliente no existe.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClienteDto>> GetById(int id)
        {
            try
            {
                var cliente = await _clienteService.GetClienteByIdAsync(id);
                return Ok(_mapper.Map<ClienteDto>(cliente));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Registra un nuevo cliente.
        /// </summary>
        /// <param name="createClienteDto">Datos del nuevo cliente.</param>
        /// <returns>El cliente creado.</returns>
        /// <response code="201">Cliente creado exitosamente.</response>
        /// <response code="400">Si los datos son inválidos.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClienteDto>> Create([FromBody] CreateClienteDto createClienteDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(createClienteDto);
                var nuevoCliente = await _clienteService.CreateClienteAsync(cliente);
                return CreatedAtAction(nameof(GetById), new { id = nuevoCliente.Id }, _mapper.Map<ClienteDto>(nuevoCliente));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza la información de un cliente.
        /// </summary>
        /// <param name="id">El ID del cliente a actualizar.</param>
        /// <param name="updateClienteDto">Los nuevos datos del cliente.</param>
        /// <returns>El cliente actualizado.</returns>
        /// <response code="200">Cliente actualizado exitosamente.</response>
        /// <response code="400">Si los datos son inválidos.</response>
        /// <response code="404">Si el cliente no existe.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClienteDto>> Update(int id, [FromBody] UpdateClienteDto updateClienteDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(updateClienteDto);
                cliente.Id = id; // Asegurar que el ID sea el correcto
                var clienteActualizado = await _clienteService.UpdateClienteAsync(cliente);
                return Ok(_mapper.Map<ClienteDto>(clienteActualizado));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el historial de citas de un cliente.
        /// </summary>
        /// <param name="id">El ID del cliente.</param>
        /// <returns>Lista de citas del cliente.</returns>
        /// <response code="200">Retorna el historial de citas.</response>
        /// <response code="404">Si el cliente no existe.</response>
        [HttpGet("{id}/historial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CitaDto>>> GetHistorial(int id)
        {
            try
            {
                var historial = await _clienteService.GetHistorialCitasAsync(id);
                return Ok(_mapper.Map<IEnumerable<CitaDto>>(historial));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
