using LocalizadorGps.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizadorGps.Infraestructura.Persistencia.Configuraciones;

public class DispositivoConfiguracion : IEntityTypeConfiguration<Dispositivo>
{
    public void Configure(EntityTypeBuilder<Dispositivo> builder)
    {
        builder.ToTable("Dispositivos");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.IdentificadorUnico)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Descripcion)
            .HasMaxLength(250);

        builder.HasIndex(d => d.IdentificadorUnico)
            .IsUnique();

        builder.HasMany(d => d.Usuarios)
            .WithOne(u => u.Dispositivo)
            .HasForeignKey(u => u.DispositivoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Ubicaciones)
            .WithOne(u => u.Dispositivo)
            .HasForeignKey(u => u.DispositivoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
