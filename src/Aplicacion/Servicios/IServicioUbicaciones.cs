using System;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Aplicacion.DTOs;

namespace LocalizadorGps.Aplicacion.Servicios;

public interface IServicioUbicaciones
{
    Task RegistrarUbicacionAsync(RegistrarUbicacionDto dto, CancellationToken ct = default);

    Task<UbicacionDto?> ObtenerUltimaUbicacionAsync(Guid vehiculoId, CancellationToken ct = default);

    Task<HistorialUbicacionesDto> ObtenerHistorialAsync(Guid vehiculoId, DateTime desdeUtc, DateTime hastaUtc, CancellationToken ct = default);
}
