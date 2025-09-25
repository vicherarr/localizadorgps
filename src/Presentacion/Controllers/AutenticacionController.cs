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
[Produces("application/json")]
public class AutenticacionController : ControllerBase
{
    private readonly IServicioAutenticacion _servicioAutenticacion;

    public AutenticacionController(IServicioAutenticacion servicioAutenticacion)
    {
        _servicioAutenticacion = servicioAutenticacion;
    }

    /// <summary>
    /// Registra un nuevo dispositivo asociado a un vehículo.
    /// </summary>
    [HttpPost("registro")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(RespuestaRegistroDispositivoDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<RespuestaRegistroDispositivoDto>> RegistrarDispositivo([FromBody] SolicitudRegistroDispositivoDto dto, CancellationToken ct)
    {
        var respuesta = await _servicioAutenticacion.RegistrarDispositivoAsync(dto, ct);
        return CreatedAtAction(nameof(RegistrarDispositivo), new { id = respuesta.DispositivoId }, respuesta);
    }

    /// <summary>
    /// Genera un token JWT válido para el dispositivo.
    /// </summary>
    [HttpPost("inicio-sesion")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RespuestaInicioSesionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RespuestaInicioSesionDto>> IniciarSesion([FromBody] SolicitudInicioSesionDto dto, CancellationToken ct)
    {
        var respuesta = await _servicioAutenticacion.IniciarSesionAsync(dto, ct);
        return Ok(respuesta);
    }
}
