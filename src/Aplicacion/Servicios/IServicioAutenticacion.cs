using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Aplicacion.DTOs;

namespace LocalizadorGps.Aplicacion.Servicios;

public interface IServicioAutenticacion
{
    Task<RespuestaRegistroDispositivoDto> RegistrarDispositivoAsync(SolicitudRegistroDispositivoDto solicitud, CancellationToken ct = default);

    Task<RespuestaInicioSesionDto> IniciarSesionAsync(SolicitudInicioSesionDto solicitud, CancellationToken ct = default);
}
