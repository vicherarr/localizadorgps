using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using LocalizadorGps.Aplicacion.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LocalizadorGps.Presentacion.Filtros;

/// <summary>
/// Convierte excepciones conocidas en respuestas JSON homogéneas.
/// </summary>
public class ManejadorExcepcionesFiltro : IExceptionFilter
{
    private readonly ILogger<ManejadorExcepcionesFiltro> _logger;

    public ManejadorExcepcionesFiltro(ILogger<ManejadorExcepcionesFiltro> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled)
        {
            return;
        }

        ProblemDetails problema;
        int statusCode;

        switch (context.Exception)
        {
            case ReglaDeNegocioException regla:
                statusCode = StatusCodes.Status400BadRequest;
                problema = CrearProblema(regla.Message, statusCode);
                break;
            case ValidationException validacion:
                statusCode = StatusCodes.Status400BadRequest;
                problema = CrearProblema(validacion.Message, statusCode);
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                _logger.LogError(context.Exception, "Error inesperado procesando la solicitud");
                problema = CrearProblema("Se produjo un error inesperado. Intente nuevamente o contacte al soporte.", statusCode);
                break;
        }

        context.Result = new ObjectResult(problema)
        {
            StatusCode = statusCode
        };
        context.ExceptionHandled = true;
    }

    private static ProblemDetails CrearProblema(string detalle, int statusCode) => new()
    {
        Title = "Ocurrió un error al procesar la solicitud",
        Detail = detalle,
        Status = statusCode,
        Type = "https://httpstatuses.com/" + statusCode
    };
}
