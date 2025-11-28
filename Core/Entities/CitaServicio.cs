using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class CitaServicio
{
    public int IdDetalle { get; set; }

    public int IdCita { get; set; }

    public int IdServicio { get; set; }

    public decimal PrecioUnitarioMomento { get; set; }

    public virtual Cita IdCitaNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;
}
