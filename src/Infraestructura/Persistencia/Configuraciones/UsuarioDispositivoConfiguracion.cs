using LocalizadorGps.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalizadorGps.Infraestructura.Persistencia.Configuraciones;

public class UsuarioDispositivoConfiguracion : IEntityTypeConfiguration<UsuarioDispositivo>
{
    public void Configure(EntityTypeBuilder<UsuarioDispositivo> builder)
    {
        builder.ToTable("UsuariosDispositivos");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.NombreUsuario)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.HashContrasena)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(u => u.NombreUsuario)
            .IsUnique();
    }
}
