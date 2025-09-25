using System;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Aplicacion.DTOs;
using LocalizadorGps.Aplicacion.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalizadorGps.Presentacion.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class UbicacionesController : ControllerBase
{
    private readonly IServicioUbicaciones _servicioUbicaciones;

    public UbicacionesController(IServicioUbicaciones servicioUbicaciones)
    {
        _servicioUbicaciones = servicioUbicaciones;
    }

    /// <summary>
    /// Registra una nueva muestra de localización proveniente del dispositivo.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> RegistrarUbicacion([FromBody] RegistrarUbicacionDto dto, CancellationToken ct)
    {
        await _servicioUbicaciones.RegistrarUbicacionAsync(dto, ct);
        return Accepted();
    }

    /// <summary>
    /// Obtiene la última ubicación registrada para un vehículo.
    /// </summary>
    [HttpGet("vehiculos/{vehiculoId:guid}/actual")]
    [ProducesResponseType(typeof(UbicacionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UbicacionDto>> ObtenerUbicacionActual(Guid vehiculoId, CancellationToken ct)
    {
        var ubicacion = await _servicioUbicaciones.ObtenerUltimaUbicacionAsync(vehiculoId, ct);
        return ubicacion is null ? NotFound() : Ok(ubicacion);
    }

    /// <summary>
    /// Devuelve el historial de ubicaciones de un vehículo en un rango de fechas UTC.
    /// </summary>
    [HttpGet("vehiculos/{vehiculoId:guid}/historial")]
    [ProducesResponseType(typeof(HistorialUbicacionesDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<HistorialUbicacionesDto>> ObtenerHistorial(Guid vehiculoId, [FromQuery] DateTime desdeUtc, [FromQuery] DateTime hastaUtc, CancellationToken ct)
    {
        var historial = await _servicioUbicaciones.ObtenerHistorialAsync(vehiculoId, desdeUtc, hastaUtc, ct);
        return Ok(historial);
    }
}
