namespace LocalizadorGps.Infraestructura.Autenticacion;

/// <summary>
/// Representa las opciones de configuración del token JWT.
/// </summary>
public class OpcionesJwt
{
    public const string Seccion = "Jwt";

    public string ClaveSecreta { get; set; } = string.Empty;

    public string Emisor { get; set; } = string.Empty;

    public string Audiencia { get; set; } = string.Empty;

    public int MinutosExpiracion { get; set; } = 60;
}
