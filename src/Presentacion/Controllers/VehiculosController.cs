using System;
using System.Collections.Generic;
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
public class VehiculosController : ControllerBase
{
    private readonly IServicioVehiculos _servicioVehiculos;

    public VehiculosController(IServicioVehiculos servicioVehiculos)
    {
        _servicioVehiculos = servicioVehiculos;
    }

    /// <summary>
    /// Crea un vehículo y lo deja listo para asignarle dispositivos.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(VehiculoDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<VehiculoDto>> CrearVehiculo([FromBody] CrearVehiculoDto dto, CancellationToken ct)
    {
        var vehiculo = await _servicioVehiculos.CrearVehiculoAsync(dto, ct);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = vehiculo.Id }, vehiculo);
    }

    /// <summary>
    /// Obtiene la información general de un vehículo.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VehiculoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<VehiculoDto>> ObtenerPorId(Guid id, CancellationToken ct)
    {
        var vehiculo = await _servicioVehiculos.ObtenerPorIdAsync(id, ct);
        return vehiculo is null ? NotFound() : Ok(vehiculo);
    }

    /// <summary>
    /// Lista los vehículos activos con reporte de ubicación en los últimos 5 minutos.
    /// </summary>
    [HttpGet("activos")]
    [ProducesResponseType(typeof(IEnumerable<VehiculoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VehiculoDto>>> ListarVehiculosActivosRecientes(CancellationToken ct)
    {
        var vehiculos = await _servicioVehiculos.ObtenerVehiculosConUbicacionRecienteAsync(TimeSpan.FromMinutes(5), ct);
        return Ok(vehiculos);
    }

    /// <summary>
    /// Actualiza la descripción o el estado de un vehículo.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarVehiculoDto dto, CancellationToken ct)
    {
        await _servicioVehiculos.ActualizarVehiculoAsync(id, dto, ct);
        return NoContent();
    }
}
