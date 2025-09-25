using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Aplicacion.Interfaces;
using LocalizadorGps.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LocalizadorGps.Infraestructura.Persistencia.Repositorios;

internal class UbicacionRepositorio : IUbicacionRepositorio
{
    private readonly LocalizadorGpsDbContext _contexto;

    public UbicacionRepositorio(LocalizadorGpsDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task RegistrarAsync(Ubicacion ubicacion, CancellationToken ct = default)
    {
        await _contexto.Ubicaciones.AddAsync(ubicacion, ct);
    }

    public Task<Ubicacion?> ObtenerUltimaPorVehiculoAsync(Guid vehiculoId, CancellationToken ct = default) =>
        _contexto.Ubicaciones
            .Where(u => u.VehiculoId == vehiculoId)
            .OrderByDescending(u => u.FechaMuestraUtc)
            .FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyCollection<Ubicacion>> ObtenerHistorialAsync(Guid vehiculoId, DateTime desdeUtc, DateTime hastaUtc, CancellationToken ct = default)
    {
        return await _contexto.Ubicaciones
            .Where(u => u.VehiculoId == vehiculoId && u.FechaMuestraUtc >= desdeUtc && u.FechaMuestraUtc <= hastaUtc)
            .OrderBy(u => u.FechaMuestraUtc)
            .AsNoTracking()
            .ToListAsync(ct);
    }
}
