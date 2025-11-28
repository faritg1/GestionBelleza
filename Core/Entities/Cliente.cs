using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Cliente : BaseEntity
{
    //public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? CorreoElectronico { get; set; }

    public string? Alias { get; set; }

    public string? Cedula { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
