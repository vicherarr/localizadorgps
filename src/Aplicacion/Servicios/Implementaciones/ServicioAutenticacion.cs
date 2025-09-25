using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Aplicacion.DTOs;
using LocalizadorGps.Aplicacion.Excepciones;
using LocalizadorGps.Aplicacion.Interfaces;
using LocalizadorGps.Dominio.Entidades;
using LocalizadorGps.Dominio.Enumeraciones;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LocalizadorGps.Aplicacion.Servicios.Implementaciones;

/// <summary>
/// Implementación de alto nivel que coordina el registro y la autenticación de dispositivos.
/// </summary>
public class ServicioAutenticacion : IServicioAutenticacion
{
    private readonly IDispositivoRepositorio _dispositivoRepositorio;
    private readonly IVehiculoRepositorio _vehiculoRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IPasswordHasher<UsuarioDispositivo> _passwordHasher;
    private readonly ITokenJwtServicio _tokenJwtServicio;
    private readonly ILogger<ServicioAutenticacion> _logger;

    public ServicioAutenticacion(
        IDispositivoRepositorio dispositivoRepositorio,
        IVehiculoRepositorio vehiculoRepositorio,
        IUnidadDeTrabajo unidadDeTrabajo,
        IPasswordHasher<UsuarioDispositivo> passwordHasher,
        ITokenJwtServicio tokenJwtServicio,
        ILogger<ServicioAutenticacion> logger)
    {
        _dispositivoRepositorio = dispositivoRepositorio;
        _vehiculoRepositorio = vehiculoRepositorio;
        _unidadDeTrabajo = unidadDeTrabajo;
        _passwordHasher = passwordHasher;
        _tokenJwtServicio = tokenJwtServicio;
        _logger = logger;
    }

    public async Task<RespuestaRegistroDispositivoDto> RegistrarDispositivoAsync(SolicitudRegistroDispositivoDto solicitud, CancellationToken ct = default)
    {
        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(solicitud.VehiculoId, ct)
            ?? throw new ReglaDeNegocioException("El vehículo especificado no existe.");

        var usuarioExistente = await _dispositivoRepositorio.ObtenerUsuarioPorNombreAsync(solicitud.NombreUsuario, ct);
        if (usuarioExistente is not null)
        {
            throw new ReglaDeNegocioException("Ya existe un usuario registrado con el nombre proporcionado.");
        }

        var dispositivo = new Dispositivo
        {
            VehiculoId = vehiculo.Id,
            Descripcion = solicitud.DescripcionDispositivo,
            IdentificadorUnico = solicitud.IdentificadorUnico
        };

        var usuario = new UsuarioDispositivo
        {
            Dispositivo = dispositivo,
            DispositivoId = dispositivo.Id,
            NombreUsuario = solicitud.NombreUsuario,
            Rol = RolDispositivo.Dispositivo
        };
        usuario.HashContrasena = _passwordHasher.HashPassword(usuario, solicitud.Contrasena);

        dispositivo.Usuarios.Add(usuario);

        await _dispositivoRepositorio.RegistrarAsync(dispositivo, usuario, ct);
        await _unidadDeTrabajo.GuardarCambiosAsync(ct);

        _logger.LogInformation("Dispositivo {DispositivoId} registrado para el vehículo {VehiculoId}.", dispositivo.Id, vehiculo.Id);

        return new RespuestaRegistroDispositivoDto
        {
            DispositivoId = dispositivo.Id,
            NombreUsuario = usuario.NombreUsuario
        };
    }

    public async Task<RespuestaInicioSesionDto> IniciarSesionAsync(SolicitudInicioSesionDto solicitud, CancellationToken ct = default)
    {
        var usuario = await _dispositivoRepositorio.ObtenerUsuarioPorNombreAsync(solicitud.NombreUsuario, ct)
            ?? throw new ReglaDeNegocioException("Las credenciales son inválidas.");

        if (!usuario.Activo)
        {
            throw new ReglaDeNegocioException("El usuario del dispositivo se encuentra deshabilitado.");
        }

        var verificacion = _passwordHasher.VerifyHashedPassword(usuario, usuario.HashContrasena, solicitud.Contrasena);
        if (verificacion == PasswordVerificationResult.Failed)
        {
            throw new ReglaDeNegocioException("Las credenciales son inválidas.");
        }

        var dispositivo = usuario.Dispositivo;
        if (!dispositivo.Activo)
        {
            throw new ReglaDeNegocioException("El dispositivo se encuentra deshabilitado por un administrador.");
        }

        dispositivo.FechaUltimoAccesoUtc = DateTime.UtcNow;
        await _dispositivoRepositorio.ActualizarAsync(dispositivo, ct);

        var expiracion = DateTime.UtcNow.AddHours(12);
        var roles = new List<string> { usuario.Rol.ToString() };
        var token = _tokenJwtServicio.GenerarToken(dispositivo.Id, dispositivo.VehiculoId, usuario.NombreUsuario, roles, expiracion);

        await _unidadDeTrabajo.GuardarCambiosAsync(ct);

        return new RespuestaInicioSesionDto
        {
            Token = token,
            ExpiraEnUtc = expiracion,
            DispositivoId = dispositivo.Id,
            VehiculoId = dispositivo.VehiculoId
        };
    }
}
