using System;
using System.ComponentModel.DataAnnotations;

namespace LocalizadorGps.Aplicacion.DTOs;

/// <summary>
/// Solicitud para registrar un dispositivo nuevo dentro del sistema.
/// </summary>
public class SolicitudRegistroDispositivoDto
{
    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string NombreUsuario { get; set; } = null!;

    [Required]
    [StringLength(200, MinimumLength = 6)]
    public string Contrasena { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string IdentificadorUnico { get; set; } = null!;

    [Required]
    public Guid VehiculoId { get; set; }

    [StringLength(250)]
    public string? DescripcionDispositivo { get; set; }
}

/// <summary>
/// Respuesta después de registrar un dispositivo.
/// </summary>
public class RespuestaRegistroDispositivoDto
{
    public Guid DispositivoId { get; set; }

    public string NombreUsuario { get; set; } = null!;
}

/// <summary>
/// Solicitud para obtener un token JWT.
/// </summary>
public class SolicitudInicioSesionDto
{
    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string NombreUsuario { get; set; } = null!;

    [Required]
    [StringLength(200, MinimumLength = 6)]
    public string Contrasena { get; set; } = null!;
}

/// <summary>
/// Respuesta enviada al autenticarse correctamente.
/// </summary>
public class RespuestaInicioSesionDto
{
    public string Token { get; set; } = null!;

    public DateTime ExpiraEnUtc { get; set; }

    public Guid DispositivoId { get; set; }

    public Guid VehiculoId { get; set; }
}
