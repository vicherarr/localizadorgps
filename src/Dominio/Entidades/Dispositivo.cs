using System;
using System.Collections.Generic;
using System.Linq;
using LocalizadorGps.Dominio.Enumeraciones;

namespace LocalizadorGps.Dominio.Entidades;

/// <summary>
/// Representa el dispositivo físico que envía los datos de localización.
/// </summary>
public class Dispositivo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identificador único proporcionado por el fabricante (IMEI, número de serie, etc.).
    /// </summary>
    public string IdentificadorUnico { get; set; } = null!;

    /// <summary>
    /// Descripción libre del dispositivo (modelo, versión de Android, etc.).
    /// </summary>
    public string? Descripcion { get; set; }

    public Guid VehiculoId { get; set; }

    public Vehiculo Vehiculo { get; set; } = null!;

    /// <summary>
    /// Fecha y hora del último acceso exitoso al sistema.
    /// </summary>
    public DateTime? FechaUltimoAccesoUtc { get; set; }

    /// <summary>
    /// Indica si el dispositivo puede continuar enviando ubicaciones.
    /// </summary>
    public bool Activo { get; private set; } = true;

    public ICollection<UsuarioDispositivo> Usuarios { get; set; } = new HashSet<UsuarioDispositivo>();

    public ICollection<Ubicacion> Ubicaciones { get; set; } = new HashSet<Ubicacion>();

    public void MarcarComoActivo() => Activo = true;

    public void MarcarComoInactivo() => Activo = false;

    public UsuarioDispositivo? ObtenerAdministradorPrincipal() =>
        Usuarios.FirstOrDefault(u => u.Rol == RolDispositivo.Administrador);
}
