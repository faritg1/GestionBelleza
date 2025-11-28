using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Usuario : BaseEntity
{
    //public int IdUsuario { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Rol { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
