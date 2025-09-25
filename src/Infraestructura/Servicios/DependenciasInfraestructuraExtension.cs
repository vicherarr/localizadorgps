using System;
using LocalizadorGps.Aplicacion.Interfaces;
using LocalizadorGps.Aplicacion.Servicios;
using LocalizadorGps.Aplicacion.Servicios.Implementaciones;
using LocalizadorGps.Dominio.Entidades;
using LocalizadorGps.Infraestructura.Autenticacion;
using LocalizadorGps.Infraestructura.Persistencia;
using LocalizadorGps.Infraestructura.Persistencia.Repositorios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocalizadorGps.Infraestructura.Servicios;

public static class DependenciasInfraestructuraExtension
{
    public static IServiceCollection AgregarInfraestructura(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpcionesJwt>(configuration.GetSection(OpcionesJwt.Seccion));

        services.AddDbContext<LocalizadorGpsDbContext>((sp, opciones) =>
        {
            var cadena = configuration.GetConnectionString("LocalizadorGps") ?? "";
            if (string.IsNullOrWhiteSpace(cadena))
            {
                opciones.UseInMemoryDatabase("LocalizadorGps");
            }
            else
            {
                opciones.UseSqlServer(cadena, sql => sql.MigrationsAssembly(typeof(LocalizadorGpsDbContext).Assembly.FullName));
            }
        });

        services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
        services.AddScoped<IVehiculoRepositorio, VehiculoRepositorio>();
        services.AddScoped<IDispositivoRepositorio, DispositivoRepositorio>();
        services.AddScoped<IUbicacionRepositorio, UbicacionRepositorio>();

        services.AddScoped<ITokenJwtServicio, TokenJwtServicio>();
        services.AddScoped<IPasswordHasher<UsuarioDispositivo>, PasswordHasher<UsuarioDispositivo>>();

        services.AddScoped<IServicioAutenticacion, ServicioAutenticacion>();
        services.AddScoped<IServicioVehiculos, ServicioVehiculos>();
        services.AddScoped<IServicioUbicaciones, ServicioUbicaciones>();

        return services;
    }
}
