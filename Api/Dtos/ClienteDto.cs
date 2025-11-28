namespace Api.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Alias { get; set; }
        public string? Cedula { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class CreateClienteDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Alias { get; set; }
        public string? Cedula { get; set; }
    }

    public class UpdateClienteDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Alias { get; set; }
        public string? Cedula { get; set; }
    }
}
