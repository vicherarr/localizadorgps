using System;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Dominio.Entidades;

namespace LocalizadorGps.Aplicacion.Interfaces;

public interface IDispositivoRepositorio
{
    Task<Dispositivo?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);

    Task<Dispositivo?> ObtenerPorIdentificadorAsync(string identificador, CancellationToken ct = default);

    Task<UsuarioDispositivo?> ObtenerUsuarioPorNombreAsync(string nombreUsuario, CancellationToken ct = default);

    Task RegistrarAsync(Dispositivo dispositivo, UsuarioDispositivo usuario, CancellationToken ct = default);

    Task ActualizarAsync(Dispositivo dispositivo, CancellationToken ct = default);

    Task ActualizarUsuarioAsync(UsuarioDispositivo usuario, CancellationToken ct = default);
}
