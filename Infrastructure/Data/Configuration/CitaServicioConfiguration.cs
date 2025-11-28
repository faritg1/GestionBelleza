using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class CitaServicioConfiguration : IEntityTypeConfiguration<CitaServicio>
    {
        public void Configure(EntityTypeBuilder<CitaServicio> entity)
        {
            entity.HasKey(e => e.IdDetalle).HasName("PRIMARY");

            entity.ToTable("cita_servicios");

            entity.HasIndex(e => e.IdCita, "fk_detalle_cita");

            entity.HasIndex(e => e.IdServicio, "fk_detalle_servicio");

            entity.Property(e => e.IdDetalle)
                .HasColumnType("int(11)")
                .HasColumnName("id_detalle");
            entity.Property(e => e.IdCita)
                .HasColumnType("int(11)")
                .HasColumnName("id_cita");
            entity.Property(e => e.IdServicio)
                .HasColumnType("int(11)")
                .HasColumnName("id_servicio");
            entity.Property(e => e.PrecioUnitarioMomento)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario_momento");

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.CitaServicios)
                .HasForeignKey(d => d.IdCita)
                .HasConstraintName("fk_detalle_cita");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.CitaServicios)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_servicio");
        }
    }
}