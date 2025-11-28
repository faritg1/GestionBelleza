using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Pago : BaseEntity
{
    //public int IdPago { get; set; }

    public int IdCita { get; set; }

    public string MetodoPago { get; set; } = null!;

    public decimal Monto { get; set; }

    public DateTime? FechaPago { get; set; }

    public string? ReferenciaPago { get; set; }

    public virtual Cita IdCitaNavigation { get; set; } = null!;
}
