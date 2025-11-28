using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Servicio : BaseEntity
{
    //public int IdServicio { get; set; }

    public string NombreServicio { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int DuracionEstimadaMin { get; set; }

    public string? Categoria { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<CitaServicio> CitaServicios { get; set; } = new List<CitaServicio>();
}
