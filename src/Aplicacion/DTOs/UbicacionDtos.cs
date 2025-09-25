using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocalizadorGps.Aplicacion.DTOs;

public class RegistrarUbicacionDto
{
    [Required]
    public Guid VehiculoId { get; set; }

    [Required]
    public Guid DispositivoId { get; set; }

    [Required]
    [Range(-90, 90)]
    public double Latitud { get; set; }

    [Required]
    [Range(-180, 180)]
    public double Longitud { get; set; }

    [Range(-500, 9000)]
    public double? Altitud { get; set; }

    [Range(0, 400)]
    public double? Velocidad { get; set; }

    [Range(0, 1000)]
    public double? Precision { get; set; }

    [Required]
    public DateTime FechaMuestraUtc { get; set; }
}

public class UbicacionDto
{
    public Guid Id { get; set; }

    public double Latitud { get; set; }

    public double Longitud { get; set; }

    public double? Altitud { get; set; }

    public double? Velocidad { get; set; }

    public double? Precision { get; set; }

    public DateTime FechaMuestraUtc { get; set; }

    public DateTime FechaRegistroUtc { get; set; }
}

public class HistorialUbicacionesDto
{
    public Guid VehiculoId { get; set; }

    public string Placa { get; set; } = null!;

    public IReadOnlyCollection<UbicacionDto> Ubicaciones { get; set; } = Array.Empty<UbicacionDto>();
}
