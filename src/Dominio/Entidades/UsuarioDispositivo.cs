using System;
using LocalizadorGps.Dominio.Enumeraciones;

namespace LocalizadorGps.Dominio.Entidades;

/// <summary>
/// Representa las credenciales de acceso que utiliza un dispositivo para autenticarse.
/// </summary>
public class UsuarioDispositivo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid DispositivoId { get; set; }

    public Dispositivo Dispositivo { get; set; } = null!;

    /// <summary>
    /// Nombre único utilizado por la aplicación Android para autenticarse.
    /// </summary>
    public string NombreUsuario { get; set; } = null!;

    /// <summary>
    /// Hash de la contraseña generado con un algoritmo seguro.
    /// </summary>
    public string HashContrasena { get; set; } = null!;

    public RolDispositivo Rol { get; set; } = RolDispositivo.Dispositivo;

    public bool Activo { get; private set; } = true;

    public void MarcarComoActivo() => Activo = true;

    public void MarcarComoInactivo() => Activo = false;
}
