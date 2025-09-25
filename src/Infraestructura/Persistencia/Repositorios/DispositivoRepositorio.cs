using System;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Aplicacion.Interfaces;
using LocalizadorGps.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LocalizadorGps.Infraestructura.Persistencia.Repositorios;

internal class DispositivoRepositorio : IDispositivoRepositorio
{
    private readonly LocalizadorGpsDbContext _contexto;

    public DispositivoRepositorio(LocalizadorGpsDbContext contexto)
    {
        _contexto = contexto;
    }

    public Task<Dispositivo?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default) =>
        _contexto.Dispositivos
            .Include(d => d.Usuarios)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

    public Task<Dispositivo?> ObtenerPorIdentificadorAsync(string identificador, CancellationToken ct = default) =>
        _contexto.Dispositivos.FirstOrDefaultAsync(d => d.IdentificadorUnico == identificador, ct);

    public Task<UsuarioDispositivo?> ObtenerUsuarioPorNombreAsync(string nombreUsuario, CancellationToken ct = default) =>
        _contexto.UsuariosDispositivos
            .Include(u => u.Dispositivo)
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario, ct);

    public async Task RegistrarAsync(Dispositivo dispositivo, UsuarioDispositivo usuario, CancellationToken ct = default)
    {
        await _contexto.Dispositivos.AddAsync(dispositivo, ct);
        await _contexto.UsuariosDispositivos.AddAsync(usuario, ct);
    }

    public Task ActualizarAsync(Dispositivo dispositivo, CancellationToken ct = default)
    {
        _contexto.Dispositivos.Update(dispositivo);
        return Task.CompletedTask;
    }

    public Task ActualizarUsuarioAsync(UsuarioDispositivo usuario, CancellationToken ct = default)
    {
        _contexto.UsuariosDispositivos.Update(usuario);
        return Task.CompletedTask;
    }
}
