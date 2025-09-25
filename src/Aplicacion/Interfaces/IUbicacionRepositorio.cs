using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Dominio.Entidades;

namespace LocalizadorGps.Aplicacion.Interfaces;

public interface IUbicacionRepositorio
{
    Task RegistrarAsync(Ubicacion ubicacion, CancellationToken ct = default);

    Task<Ubicacion?> ObtenerUltimaPorVehiculoAsync(Guid vehiculoId, CancellationToken ct = default);

    Task<IReadOnlyCollection<Ubicacion>> ObtenerHistorialAsync(Guid vehiculoId, DateTime desdeUtc, DateTime hastaUtc, CancellationToken ct = default);
}
