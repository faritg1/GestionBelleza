using Api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de usuarios.
    /// </summary>
    public class UsuarioController : BaseControllerNoAuth
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario.</param>
        /// <returns>El usuario solicitado.</returns>
        /// <response code="200">Retorna el usuario.</response>
        /// <response code="404">Si el usuario no existe.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
                return Ok(_mapper.Map<UsuarioDto>(usuario));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Crea un nuevo usuario (solo admin).
        /// </summary>
        /// <param name="createUsuarioDto">Datos del nuevo usuario.</param>
        /// <returns>El usuario creado.</returns>
        /// <response code="201">Usuario creado exitosamente.</response>
        /// <response code="400">Si los datos son inválidos.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsuarioDto>> Create([FromBody] CreateUsuarioDto createUsuarioDto)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(createUsuarioDto);
                var nuevoUsuario = await _usuarioService.CreateUsuarioAsync(usuario, createUsuarioDto.Password);
                return CreatedAtAction(nameof(GetById), new { id = nuevoUsuario.Id }, _mapper.Map<UsuarioDto>(nuevoUsuario));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cambia la contraseña del usuario autenticado.
        /// </summary>
        /// <param name="cambiarPasswordDto">Datos para el cambio de contraseña.</param>
        /// <returns>Mensaje de éxito.</returns>
        /// <response code="200">Contraseña actualizada correctamente.</response>
        /// <response code="400">Si la contraseña actual es incorrecta o la nueva no cumple los requisitos.</response>
        /// <response code="401">Si el usuario no está autenticado.</response>
        [HttpPost("cambiar-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CambiarPassword([FromBody] CambiarPasswordDto cambiarPasswordDto)
        {
            try
            {
                // Obtener ID del usuario del token
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
                if (userIdClaim == null) return Unauthorized();
                
                int userId = int.Parse(userIdClaim.Value);

                await _usuarioService.CambiarPasswordAsync(userId, cambiarPasswordDto.PasswordActual, cambiarPasswordDto.PasswordNuevo);
                return Ok("Contraseña actualizada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
