using LocalizadorGps.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizadorGps.Infraestructura.Persistencia.Configuraciones;

public class UbicacionConfiguracion : IEntityTypeConfiguration<Ubicacion>
{
    public void Configure(EntityTypeBuilder<Ubicacion> builder)
    {
        builder.ToTable("Ubicaciones");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Latitud)
            .HasPrecision(12, 9);

        builder.Property(u => u.Longitud)
            .HasPrecision(12, 9);

        builder.Property(u => u.Altitud)
            .HasPrecision(10, 2);

        builder.Property(u => u.Velocidad)
            .HasPrecision(10, 2);

        builder.Property(u => u.Precision)
            .HasPrecision(10, 2);

        builder.HasIndex(u => new { u.VehiculoId, u.FechaMuestraUtc });
    }
}
