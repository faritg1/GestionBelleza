namespace Api.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
        public string? Rol { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CreateUsuarioDto
    {
        public string NombreCompleto { get; set; }
        public string CorreoElectronico { get; set; }
        public string Password { get; set; }
        public string? Telefono { get; set; }
        public string? Rol { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; }
    }

    public class CambiarPasswordDto
    {
        public string PasswordActual { get; set; }
        public string PasswordNuevo { get; set; }
    }
}
