using System;
using System.ComponentModel.DataAnnotations;

namespace LocalizadorGps.Aplicacion.DTOs;

public class CrearVehiculoDto
{
    [Required]
    [StringLength(12, MinimumLength = 5)]
    public string Placa { get; set; } = null!;

    [StringLength(200)]
    public string? Descripcion { get; set; }
}

public class ActualizarVehiculoDto
{
    [Required]
    [StringLength(200)]
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;
}

public class VehiculoDto
{
    public Guid Id { get; set; }

    public string Placa { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public DateTime? UltimaUbicacionUtc { get; set; }

    public double? UltimaLatitud { get; set; }

    public double? UltimaLongitud { get; set; }
}
