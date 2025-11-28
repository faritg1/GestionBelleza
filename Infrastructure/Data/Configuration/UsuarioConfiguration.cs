using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.CorreoElectronico, "correo_electronico").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .HasColumnName("contrasena");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(100)
                .HasColumnName("correo_electronico");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(100)
                .HasColumnName("nombre_completo");
            entity.Property(e => e.Rol)
                .HasDefaultValueSql("'admin'")
                .HasColumnType("enum('admin','empleado')")
                .HasColumnName("rol");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        }
    }
}