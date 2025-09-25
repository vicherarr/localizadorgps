using System;
using System.Collections.Generic;

namespace LocalizadorGps.Dominio.Entidades;

/// <summary>
/// Entidad raíz que representa un vehículo dentro de la flota.
/// </summary>
public class Vehiculo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Placa o matrícula única del vehículo.
    /// </summary>
    public string Placa { get; set; } = null!;

    /// <summary>
    /// Descripción libre para identificar el vehículo (modelo, color, etc.).
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Indica si el vehículo se encuentra activo dentro del sistema.
    /// </summary>
    public bool Activo { get; private set; } = true;

    /// <summary>
    /// Relación con los dispositivos asignados al vehículo.
    /// </summary>
    public ICollection<Dispositivo> Dispositivos { get; set; } = new HashSet<Dispositivo>();

    /// <summary>
    /// Registro histórico de ubicaciones capturadas para el vehículo.
    /// </summary>
    public ICollection<Ubicacion> Ubicaciones { get; set; } = new HashSet<Ubicacion>();

    public void MarcarComoActivo() => Activo = true;

    public void MarcarComoInactivo() => Activo = false;
}
