using LocalizadorGps.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LocalizadorGps.Infraestructura.Persistencia;

/// <summary>
/// Contexto principal de Entity Framework Core encargado de mapear las entidades del dominio.
/// </summary>
public class LocalizadorGpsDbContext : DbContext
{
    public LocalizadorGpsDbContext(DbContextOptions<LocalizadorGpsDbContext> options) : base(options)
    {
    }

    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();

    public DbSet<Dispositivo> Dispositivos => Set<Dispositivo>();

    public DbSet<UsuarioDispositivo> UsuariosDispositivos => Set<UsuarioDispositivo>();

    public DbSet<Ubicacion> Ubicaciones => Set<Ubicacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocalizadorGpsDbContext).Assembly);
    }
}
