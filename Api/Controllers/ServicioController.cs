using Api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de servicios.
    /// </summary>
    public class ServicioController : BaseController
    {
        private readonly IServicioService _servicioService;
        private readonly IMapper _mapper;

        public ServicioController(IServicioService servicioService, IMapper mapper)
        {
            _servicioService = servicioService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los servicios registrados.
        /// </summary>
        /// <returns>Lista de todos los servicios.</returns>
        /// <response code="200">Retorna la lista de servicios.</response>
        [HttpGet]
        [AllowAnonymous] // Permitir ver servicios sin login (opcional, según requerimiento)
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetAll()
        {
            var servicios = await _servicioService.GetAllServiciosAsync();
            return Ok(_mapper.Map<IEnumerable<ServicioDto>>(servicios));
        }

        /// <summary>
        /// Obtiene solo los servicios activos.
        /// </summary>
        /// <returns>Lista de servicios activos.</returns>
        /// <response code="200">Retorna la lista de servicios activos.</response>
        [HttpGet("activos")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetActivos()
        {
            var servicios = await _servicioService.GetServiciosActivosAsync();
            return Ok(_mapper.Map<IEnumerable<ServicioDto>>(servicios));
        }

        /// <summary>
        /// Obtiene un servicio por su ID.
        /// </summary>
        /// <param name="id">El ID del servicio.</param>
        /// <returns>El servicio solicitado.</returns>
        /// <response code="200">Retorna el servicio.</response>
        /// <response code="404">Si el servicio no existe.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServicioDto>> GetById(int id)
        {
            try
            {
                var servicio = await _servicioService.GetServicioByIdAsync(id);
                return Ok(_mapper.Map<ServicioDto>(servicio));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Crea un nuevo servicio.
        /// </summary>
        /// <param name="createServicioDto">Datos del nuevo servicio.</param>
        /// <returns>El servicio creado.</returns>
        /// <response code="201">Servicio creado exitosamente.</response>
        /// <response code="400">Si los datos son inválidos.</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServicioDto>> Create([FromBody] CreateServicioDto createServicioDto)
        {
            try
            {
                var servicio = _mapper.Map<Servicio>(createServicioDto);
                var nuevoServicio = await _servicioService.CreateServicioAsync(servicio);
                return CreatedAtAction(nameof(GetById), new { id = nuevoServicio.Id }, _mapper.Map<ServicioDto>(nuevoServicio));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza la información de un servicio.
        /// </summary>
        /// <param name="id">El ID del servicio a actualizar.</param>
        /// <param name="updateServicioDto">Los nuevos datos del servicio.</param>
        /// <returns>El servicio actualizado.</returns>
        /// <response code="200">Servicio actualizado exitosamente.</response>
        /// <response code="400">Si los datos son inválidos.</response>
        /// <response code="404">Si el servicio no existe.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServicioDto>> Update(int id, [FromBody] UpdateServicioDto updateServicioDto)
        {
            try
            {
                var servicio = _mapper.Map<Servicio>(updateServicioDto);
                servicio.Id = id;
                var servicioActualizado = await _servicioService.UpdateServicioAsync(servicio);
                return Ok(_mapper.Map<ServicioDto>(servicioActualizado));
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
        /// Actualiza el precio de un servicio.
        /// </summary>
        /// <param name="id">El ID del servicio.</param>
        /// <param name="updatePrecioDto">El nuevo precio.</param>
        /// <returns>El servicio con el precio actualizado.</returns>
        /// <response code="200">Precio actualizado exitosamente.</response>
        /// <response code="400">Si el precio no es válido.</response>
        /// <response code="404">Si el servicio no existe.</response>
        [HttpPatch("{id}/precio")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServicioDto>> UpdatePrecio(int id, [FromBody] UpdatePrecioDto updatePrecioDto)
        {
            try
            {
                var servicioActualizado = await _servicioService.UpdatePrecioAsync(id, updatePrecioDto.NuevoPrecio);
                return Ok(_mapper.Map<ServicioDto>(servicioActualizado));
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
    }
}
