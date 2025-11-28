using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.Cedula, "cedula").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id_cliente");
            entity.Property(e => e.Alias)
                .HasMaxLength(50)
                .HasColumnName("alias");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .HasColumnName("apellido");
            entity.Property(e => e.Cedula)
                .HasMaxLength(20)
                .HasColumnName("cedula");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(100)
                .HasColumnName("correo_electronico");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        }
    }
}