using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Cita : BaseEntity
{
    //public int IdCita { get; set; }

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

    public virtual ICollection<CitaServicio> CitaServicios { get; set; } = new List<CitaServicio>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
