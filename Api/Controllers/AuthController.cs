using Api.Dtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador para la autenticación de usuarios.
    /// </summary>
    public class AuthController : BaseControllerNoAuth
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;

        public AuthController(IUsuarioService usuarioService, ITokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Inicia sesión en el sistema.
        /// </summary>
        /// <param name="loginDto">Objeto con las credenciales del usuario (email y contraseña).</param>
        /// <returns>Un objeto con la información del usuario y el token JWT.</returns>
        /// <response code="200">Inicio de sesión exitoso.</response>
        /// <response code="401">Credenciales inválidas.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var isValid = await _usuarioService.ValidarCredencialesAsync(loginDto.Email, loginDto.Password);
            
            if (!isValid)
            {
                return Unauthorized("Credenciales inválidas");
            }

            var usuario = await _usuarioService.GetUsuarioByEmailAsync(loginDto.Email);
            var token = _tokenService.CreateToken(usuario);

            var response = new LoginResponseDto
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.CorreoElectronico,
                Rol = usuario.Rol,
                Token = token
            };

            return Ok(response);
        }
    }
}
