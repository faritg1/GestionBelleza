using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class CitaConfiguration : IEntityTypeConfiguration<Cita>
    {
        public void Configure(EntityTypeBuilder<Cita> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("citas");

            entity.HasIndex(e => e.IdCliente, "fk_cita_cliente");

            entity.HasIndex(e => e.IdUsuario, "fk_cita_usuario");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id_cita");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'Pendiente'")
                .HasColumnType("enum('Pendiente','Confirmada','En Curso','Finalizada','Cancelada','Reprogramada')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCita).HasColumnName("fecha_cita");
            entity.Property(e => e.HoraFin)
                .HasColumnType("time")
                .HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio)
                .HasColumnType("time")
                .HasColumnName("hora_inicio");
            entity.Property(e => e.IdCliente)
                .HasColumnType("int(11)")
                .HasColumnName("id_cliente");
            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.NotasAdicionales)
                .HasColumnType("text")
                .HasColumnName("notas_adicionales");
            entity.Property(e => e.TotalDuracionMin)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("total_duracion_min");
            entity.Property(e => e.TotalPrecio)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("total_precio");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cita_cliente");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cita_usuario");
        }
    }
}