using Api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de citas.
    /// </summary>
    public class CitaController : BaseController
    {
        private readonly ICitaService _citaService;
        private readonly IMapper _mapper;

        public CitaController(ICitaService citaService, IMapper mapper)
        {
            _citaService = citaService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todas las citas registradas.
        /// </summary>
        /// <returns>Una lista de citas.</returns>
        /// <response code="200">Retorna la lista de citas.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CitaDto>>> GetAll()
        {
            var citas = await _citaService.GetAllCitasAsync();
            return Ok(_mapper.Map<IEnumerable<CitaDto>>(citas));
        }

        /// <summary>
        /// Obtiene una cita por su ID.
        /// </summary>
        /// <param name="id">El ID de la cita.</param>
        /// <returns>La cita solicitada.</returns>
        /// <response code="200">Retorna la cita.</response>
        /// <response code="404">Si la cita no existe.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CitaDto>> GetById(int id)
        {
            try
            {
                var cita = await _citaService.GetCitaByIdAsync(id);
                return Ok(_mapper.Map<CitaDto>(cita));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Crea una nueva cita.
        /// </summary>
        /// <param name="createCitaDto">Datos para crear la cita.</param>
        /// <returns>La cita creada.</returns>
        /// <response code="201">Cita creada exitosamente.</response>
        /// <response code="400">Si los datos son inválidos.</response>
        [HttpPost]
        [Authorize(Roles = "admin")] // RF A1: Solo admin puede programar
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CitaDto>> Create([FromBody] CreateCitaDto createCitaDto)
        {
            try
            {
                // RF A4: Capturar usuario logueado si no se envía (aunque el DTO lo pide, podríamos hacerlo opcional o validarlo)
                // En este caso, asumimos que el admin selecciona la especialista (IdUsuario en DTO)
                // Si se quisiera que el usuario logueado sea el responsable por defecto si es especialista, se podría ajustar.
                // Pero el requerimiento dice "se debe capturar automáticamente el usuario logueado... como responsable".
                // Si el admin crea la cita, ¿él es el responsable o asigna a una especialista?
                // RF A4 dice: "Al crear una cita, se debe capturar automáticamente el usuario logueado... como responsable".
                // Pero RF A1 dice: "Solo el administrador puede programar nuevas citas".
                // Y RF A4 dice: "Se registra el ID de la especialista que realiza el servicio".
                // Esto puede ser contradictorio si el admin es quien agenda pero no quien realiza el servicio.
                // Asumiremos que el admin ASIGNA la especialista (IdUsuario en el DTO).
                
                var cita = _mapper.Map<Cita>(createCitaDto);
                var nuevaCita = await _citaService.CreateCitaAsync(cita, createCitaDto.ServiciosIds);
                return CreatedAtAction(nameof(GetById), new { id = nuevaCita.Id }, _mapper.Map<CitaDto>(nuevaCita));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza el estado de una cita.
        /// </summary>
        /// <param name="id">El ID de la cita.</param>
        /// <param name="updateEstadoDto">El nuevo estado de la cita.</param>
        /// <returns>La cita actualizada.</returns>
        /// <response code="200">Estado actualizado exitosamente.</response>
        /// <response code="400">Si el estado no es válido.</response>
        /// <response code="404">Si la cita no existe.</response>
        [HttpPatch("{id}/estado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CitaDto>> UpdateEstado(int id, [FromBody] UpdateEstadoCitaDto updateEstadoDto)
        {
            try
            {
                var citaActualizada = await _citaService.UpdateEstadoCitaAsync(id, updateEstadoDto.Estado);
                return Ok(_mapper.Map<CitaDto>(citaActualizada));
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
        /// Valida la disponibilidad de un especialista en una fecha y hora.
        /// </summary>
        /// <param name="validarDto">Datos para validar la disponibilidad.</param>
        /// <returns>True si está disponible, False en caso contrario.</returns>
        /// <response code="200">Retorna el resultado de la validación.</response>
        [HttpPost("validar-disponibilidad")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ValidarDisponibilidad([FromBody] ValidarDisponibilidadDto validarDto)
        {
            var disponible = await _citaService.ValidarDisponibilidadAsync(
                validarDto.UsuarioId,
                validarDto.Fecha,
                validarDto.HoraInicio,
                validarDto.HoraFin
            );
            return Ok(disponible);
        }

        /// <summary>
        /// Obtiene las citas de una fecha específica.
        /// </summary>
        /// <param name="fecha">La fecha a consultar (formato YYYY-MM-DD).</param>
        /// <returns>Lista de citas de esa fecha.</returns>
        /// <response code="200">Retorna la lista de citas.</response>
        [HttpGet("fecha/{fecha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CitaDto>>> GetByFecha(DateOnly fecha)
        {
            var citas = await _citaService.GetCitasByFechaAsync(fecha);
            return Ok(_mapper.Map<IEnumerable<CitaDto>>(citas));
        }
    }
}