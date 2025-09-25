using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Aplicacion.Interfaces;
using LocalizadorGps.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LocalizadorGps.Infraestructura.Persistencia.Repositorios;

internal class VehiculoRepositorio : IVehiculoRepositorio
{
    private readonly LocalizadorGpsDbContext _contexto;

    public VehiculoRepositorio(LocalizadorGpsDbContext contexto)
    {
        _contexto = contexto;
    }

    public Task<Vehiculo?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default) =>
        _contexto.Vehiculos
            .Include(v => v.Ubicaciones)
            .FirstOrDefaultAsync(v => v.Id == id, ct);

    public Task<Vehiculo?> ObtenerPorPlacaAsync(string placa, CancellationToken ct = default) =>
        _contexto.Vehiculos.FirstOrDefaultAsync(v => v.Placa == placa, ct);

    public async Task<IReadOnlyCollection<Vehiculo>> ObtenerActivosAsync(CancellationToken ct = default)
    {
        return await _contexto.Vehiculos
            .Where(v => v.Activo)
            .Include(v => v.Ubicaciones)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<Vehiculo>> ObtenerVehiculosConUbicacionRecienteAsync(DateTime limiteUtc, CancellationToken ct = default)
    {
        return await _contexto.Vehiculos
            .Where(v => v.Activo && v.Ubicaciones.Any(u => u.FechaMuestraUtc >= limiteUtc))
            .Include(v => v.Ubicaciones)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task CrearAsync(Vehiculo vehiculo, CancellationToken ct = default)
    {
        await _contexto.Vehiculos.AddAsync(vehiculo, ct);
    }

    public Task ActualizarAsync(Vehiculo vehiculo, CancellationToken ct = default)
    {
        _contexto.Vehiculos.Update(vehiculo);
        return Task.CompletedTask;
    }
}
