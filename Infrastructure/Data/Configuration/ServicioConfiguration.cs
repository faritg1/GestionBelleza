using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class ServicioConfiguration : IEntityTypeConfiguration<Servicio>
    {
        public void Configure(EntityTypeBuilder<Servicio> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("servicios");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id_servicio");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("'1'")
                .HasColumnName("activo");
            entity.Property(e => e.Categoria)
                .HasMaxLength(50)
                .HasColumnName("categoria");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.DuracionEstimadaMin)
                .HasColumnType("int(11)")
                .HasColumnName("duracion_estimada_min");
            entity.Property(e => e.NombreServicio)
                .HasMaxLength(100)
                .HasColumnName("nombre_servicio");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
        }   
    }
}