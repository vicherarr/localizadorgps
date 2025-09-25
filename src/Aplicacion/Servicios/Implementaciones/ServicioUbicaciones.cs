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

public class ServicioUbicaciones : IServicioUbicaciones
{
    private readonly IUbicacionRepositorio _ubicacionRepositorio;
    private readonly IVehiculoRepositorio _vehiculoRepositorio;
    private readonly IDispositivoRepositorio _dispositivoRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly ILogger<ServicioUbicaciones> _logger;

    public ServicioUbicaciones(
        IUbicacionRepositorio ubicacionRepositorio,
        IVehiculoRepositorio vehiculoRepositorio,
        IDispositivoRepositorio dispositivoRepositorio,
        IUnidadDeTrabajo unidadDeTrabajo,
        ILogger<ServicioUbicaciones> logger)
    {
        _ubicacionRepositorio = ubicacionRepositorio;
        _vehiculoRepositorio = vehiculoRepositorio;
        _dispositivoRepositorio = dispositivoRepositorio;
        _unidadDeTrabajo = unidadDeTrabajo;
        _logger = logger;
    }

    public async Task RegistrarUbicacionAsync(RegistrarUbicacionDto dto, CancellationToken ct = default)
    {
        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(dto.VehiculoId, ct)
            ?? throw new ReglaDeNegocioException("El vehículo indicado no existe.");

        var dispositivo = await _dispositivoRepositorio.ObtenerPorIdAsync(dto.DispositivoId, ct)
            ?? throw new ReglaDeNegocioException("El dispositivo indicado no existe.");

        if (dispositivo.VehiculoId != vehiculo.Id)
        {
            throw new ReglaDeNegocioException("El dispositivo no está asociado al vehículo indicado.");
        }

        if (!vehiculo.Activo || !dispositivo.Activo)
        {
            throw new ReglaDeNegocioException("El vehículo o el dispositivo se encuentran deshabilitados.");
        }

        var ubicacion = new Ubicacion
        {
            VehiculoId = vehiculo.Id,
            DispositivoId = dispositivo.Id,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            Altitud = dto.Altitud,
            Velocidad = dto.Velocidad,
            Precision = dto.Precision,
            FechaMuestraUtc = dto.FechaMuestraUtc.ToUniversalTime(),
            FechaRegistroUtc = DateTime.UtcNow
        };

        await _ubicacionRepositorio.RegistrarAsync(ubicacion, ct);
        await _unidadDeTrabajo.GuardarCambiosAsync(ct);

        _logger.LogDebug("Ubicación {UbicacionId} registrada para el vehículo {VehiculoId}.", ubicacion.Id, vehiculo.Id);
    }

    public async Task<UbicacionDto?> ObtenerUltimaUbicacionAsync(Guid vehiculoId, CancellationToken ct = default)
    {
        var ubicacion = await _ubicacionRepositorio.ObtenerUltimaPorVehiculoAsync(vehiculoId, ct);
        return ubicacion is null ? null : Mapear(ubicacion);
    }

    public async Task<HistorialUbicacionesDto> ObtenerHistorialAsync(Guid vehiculoId, DateTime desdeUtc, DateTime hastaUtc, CancellationToken ct = default)
    {
        if (desdeUtc > hastaUtc)
        {
            throw new ReglaDeNegocioException("La fecha inicial no puede ser mayor que la fecha final.");
        }

        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(vehiculoId, ct)
            ?? throw new ReglaDeNegocioException("El vehículo indicado no existe.");

        var historial = await _ubicacionRepositorio.ObtenerHistorialAsync(vehiculoId, desdeUtc, hastaUtc, ct);

        return new HistorialUbicacionesDto
        {
            VehiculoId = vehiculo.Id,
            Placa = vehiculo.Placa,
            Ubicaciones = historial.Select(Mapear).ToList()
        };
    }

    private static UbicacionDto Mapear(Ubicacion ubicacion) => new()
    {
        Id = ubicacion.Id,
        Latitud = ubicacion.Latitud,
        Longitud = ubicacion.Longitud,
        Altitud = ubicacion.Altitud,
        Velocidad = ubicacion.Velocidad,
        Precision = ubicacion.Precision,
        FechaMuestraUtc = ubicacion.FechaMuestraUtc,
        FechaRegistroUtc = ubicacion.FechaRegistroUtc
    };
}
