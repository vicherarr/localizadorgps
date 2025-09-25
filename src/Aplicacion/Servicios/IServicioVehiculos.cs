using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Aplicacion.DTOs;

namespace LocalizadorGps.Aplicacion.Servicios;

public interface IServicioVehiculos
{
    Task<VehiculoDto> CrearVehiculoAsync(CrearVehiculoDto dto, CancellationToken ct = default);

    Task<VehiculoDto?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyCollection<VehiculoDto>> ObtenerVehiculosActivosAsync(CancellationToken ct = default);

    Task<IReadOnlyCollection<VehiculoDto>> ObtenerVehiculosConUbicacionRecienteAsync(TimeSpan ventana, CancellationToken ct = default);

    Task ActualizarVehiculoAsync(Guid id, ActualizarVehiculoDto dto, CancellationToken ct = default);
}
