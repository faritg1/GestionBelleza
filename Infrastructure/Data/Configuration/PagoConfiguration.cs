using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class PagoConfiguration : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pagos");

            entity.HasIndex(e => e.IdCita, "fk_pago_cita");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id_pago");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_pago");
            entity.Property(e => e.IdCita)
                .HasColumnType("int(11)")
                .HasColumnName("id_cita");
            entity.Property(e => e.MetodoPago)
                .HasColumnType("enum('Efectivo','Transferencia','NEQUI','DaviPlata','Tarjeta')")
                .HasColumnName("metodo_pago");
            entity.Property(e => e.Monto)
                .HasPrecision(10, 2)
                .HasColumnName("monto");
            entity.Property(e => e.ReferenciaPago)
                .HasMaxLength(100)
                .HasColumnName("referencia_pago");

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_pago_cita");
        }
    }
}