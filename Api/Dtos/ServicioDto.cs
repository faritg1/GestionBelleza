namespace Api.Dtos
{
    public class ServicioDto
    {
        public int Id { get; set; }
        public string NombreServicio { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int DuracionEstimadaMin { get; set; }
        public string? Categoria { get; set; }
        public bool? Activo { get; set; }
    }

    public class CreateServicioDto
    {
        public string NombreServicio { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int DuracionEstimadaMin { get; set; }
        public string? Categoria { get; set; }
        public bool? Activo { get; set; }
    }

    public class UpdateServicioDto
    {
        public string NombreServicio { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int DuracionEstimadaMin { get; set; }
        public string? Categoria { get; set; }
        public bool? Activo { get; set; }
    }

    public class UpdatePrecioDto
    {
        public decimal NuevoPrecio { get; set; }
    }
}
