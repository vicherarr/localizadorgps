using System;

namespace LocalizadorGps.Dominio.Entidades;

/// <summary>
/// Representa una muestra puntual de localización enviada por un dispositivo.
/// </summary>
public class Ubicacion
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid VehiculoId { get; set; }

    public Vehiculo Vehiculo { get; set; } = null!;

    public Guid DispositivoId { get; set; }

    public Dispositivo Dispositivo { get; set; } = null!;

    /// <summary>
    /// Latitud en formato decimal.
    /// </summary>
    public double Latitud { get; set; }

    /// <summary>
    /// Longitud en formato decimal.
    /// </summary>
    public double Longitud { get; set; }

    /// <summary>
    /// Altitud opcional en metros.
    /// </summary>
    public double? Altitud { get; set; }

    /// <summary>
    /// Velocidad opcional en kilómetros por hora.
    /// </summary>
    public double? Velocidad { get; set; }

    /// <summary>
    /// Precisión opcional en metros reportada por el dispositivo.
    /// </summary>
    public double? Precision { get; set; }

    /// <summary>
    /// Fecha y hora (UTC) de la muestra registrada por el dispositivo.
    /// </summary>
    public DateTime FechaMuestraUtc { get; set; }

    /// <summary>
    /// Fecha y hora (UTC) en la que la API procesó y guardó la muestra.
    /// </summary>
    public DateTime FechaRegistroUtc { get; set; } = DateTime.UtcNow;
}
