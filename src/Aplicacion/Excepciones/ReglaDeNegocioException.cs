using System;

namespace LocalizadorGps.Aplicacion.Excepciones;

/// <summary>
/// Excepción que se lanza cuando se incumple una regla de negocio conocida.
/// </summary>
public class ReglaDeNegocioException : Exception
{
    public ReglaDeNegocioException(string mensaje) : base(mensaje)
    {
    }
}
