using LocalizadorGps.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizadorGps.Infraestructura.Persistencia.Configuraciones;

public class VehiculoConfiguracion : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(EntityTypeBuilder<Vehiculo> builder)
    {
        builder.ToTable("Vehiculos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Placa)
            .IsRequired()
            .HasMaxLength(12);

        builder.Property(v => v.Descripcion)
            .HasMaxLength(200);

        builder.HasMany(v => v.Dispositivos)
            .WithOne(d => d.Vehiculo)
            .HasForeignKey(d => d.VehiculoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.Ubicaciones)
            .WithOne(u => u.Vehiculo)
            .HasForeignKey(u => u.VehiculoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
