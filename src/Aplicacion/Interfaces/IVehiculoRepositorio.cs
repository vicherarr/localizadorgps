using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Dominio.Entidades;

namespace LocalizadorGps.Aplicacion.Interfaces;

public interface IVehiculoRepositorio
{
    Task<Vehiculo?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);

    Task<Vehiculo?> ObtenerPorPlacaAsync(string placa, CancellationToken ct = default);

    Task<IReadOnlyCollection<Vehiculo>> ObtenerActivosAsync(CancellationToken ct = default);

    Task<IReadOnlyCollection<Vehiculo>> ObtenerVehiculosConUbicacionRecienteAsync(DateTime limiteUtc, CancellationToken ct = default);

    Task CrearAsync(Vehiculo vehiculo, CancellationToken ct = default);

    Task ActualizarAsync(Vehiculo vehiculo, CancellationToken ct = default);
}
