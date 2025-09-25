using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Aplicacion.DTOs;
using LocalizadorGps.Aplicacion.Excepciones;
using LocalizadorGps.Aplicacion.Interfaces;
using LocalizadorGps.Dominio.Entidades;
using Microsoft.Extensions.Logging;

namespace LocalizadorGps.Aplicacion.Servicios.Implementaciones;

public class ServicioVehiculos : IServicioVehiculos
{
    private readonly IVehiculoRepositorio _vehiculoRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly ILogger<ServicioVehiculos> _logger;

    public ServicioVehiculos(IVehiculoRepositorio vehiculoRepositorio, IUnidadDeTrabajo unidadDeTrabajo, ILogger<ServicioVehiculos> logger)
    {
        _vehiculoRepositorio = vehiculoRepositorio;
        _unidadDeTrabajo = unidadDeTrabajo;
        _logger = logger;
    }

    public async Task<VehiculoDto> CrearVehiculoAsync(CrearVehiculoDto dto, CancellationToken ct = default)
    {
        var existente = await _vehiculoRepositorio.ObtenerPorPlacaAsync(dto.Placa, ct);
        if (existente is not null)
        {
            throw new ReglaDeNegocioException("Ya existe un vehículo con la placa proporcionada.");
        }

        var vehiculo = new Vehiculo
        {
            Placa = dto.Placa.ToUpperInvariant(),
            Descripcion = dto.Descripcion
        };

        await _vehiculoRepositorio.CrearAsync(vehiculo, ct);
        await _unidadDeTrabajo.GuardarCambiosAsync(ct);

        _logger.LogInformation("Vehículo {VehiculoId} creado con placa {Placa}.", vehiculo.Id, vehiculo.Placa);

        return Mapear(vehiculo);
    }

    public async Task<VehiculoDto?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default)
    {
        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id, ct);
        return vehiculo is null ? null : Mapear(vehiculo);
    }

    public async Task<IReadOnlyCollection<VehiculoDto>> ObtenerVehiculosActivosAsync(CancellationToken ct = default)
    {
        var vehiculos = await _vehiculoRepositorio.ObtenerActivosAsync(ct);
        return vehiculos.Select(Mapear).ToList();
    }

    public async Task<IReadOnlyCollection<VehiculoDto>> ObtenerVehiculosConUbicacionRecienteAsync(TimeSpan ventana, CancellationToken ct = default)
    {
        var limite = DateTime.UtcNow.Subtract(ventana);
        var vehiculos = await _vehiculoRepositorio.ObtenerVehiculosConUbicacionRecienteAsync(limite, ct);
        return vehiculos.Select(Mapear).ToList();
    }

    public async Task ActualizarVehiculoAsync(Guid id, ActualizarVehiculoDto dto, CancellationToken ct = default)
    {
        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id, ct)
            ?? throw new ReglaDeNegocioException("El vehículo solicitado no existe.");

        vehiculo.Descripcion = dto.Descripcion;
        if (dto.Activo)
        {
            vehiculo.MarcarComoActivo();
        }
        else
        {
            vehiculo.MarcarComoInactivo();
        }

        await _vehiculoRepositorio.ActualizarAsync(vehiculo, ct);
        await _unidadDeTrabajo.GuardarCambiosAsync(ct);

        _logger.LogInformation("Vehículo {VehiculoId} actualizado.", vehiculo.Id);
    }

    private static VehiculoDto Mapear(Vehiculo vehiculo)
    {
        var ultimaUbicacion = vehiculo.Ubicaciones?.OrderByDescending(u => u.FechaMuestraUtc).FirstOrDefault();

        return new VehiculoDto
        {
            Id = vehiculo.Id,
            Placa = vehiculo.Placa,
            Descripcion = vehiculo.Descripcion,
            Activo = vehiculo.Activo,
            UltimaUbicacionUtc = ultimaUbicacion?.FechaMuestraUtc,
            UltimaLatitud = ultimaUbicacion?.Latitud,
            UltimaLongitud = ultimaUbicacion?.Longitud
        };
    }
}
