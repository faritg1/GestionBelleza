namespace Api.Dtos
{
    public class CitaDto
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public DateOnly FechaCita { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public string? Estado { get; set; }
        public decimal? TotalPrecio { get; set; }
        public int? TotalDuracionMin { get; set; }
        public string? NotasAdicionales { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Datos del cliente
        public string? NombreCliente { get; set; }
        public string? ApellidoCliente { get; set; }
        
        // Datos del usuario/especialista
        public string? NombreUsuario { get; set; }
    }

    public class CreateCitaDto
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public DateOnly FechaCita { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public List<int> ServiciosIds { get; set; }
        public string? NotasAdicionales { get; set; }
    }

    public class UpdateEstadoCitaDto
    {
        public string Estado { get; set; }
    }

    public class ValidarDisponibilidadDto
    {
        public int UsuarioId { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }
}
