using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LocalizadorGps.Aplicacion.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LocalizadorGps.Infraestructura.Autenticacion;

internal class TokenJwtServicio : ITokenJwtServicio
{
    private readonly OpcionesJwt _opciones;

    public TokenJwtServicio(IOptions<OpcionesJwt> opciones)
    {
        _opciones = opciones.Value;
    }

    public string GenerarToken(Guid dispositivoId, Guid vehiculoId, string nombreUsuario, IEnumerable<string> roles, DateTime expiracion)
    {
        var fechaExpiracion = expiracion < DateTime.UtcNow
            ? DateTime.UtcNow.AddMinutes(_opciones.MinutosExpiracion)
            : expiracion;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, nombreUsuario),
            new("dispositivoId", dispositivoId.ToString()),
            new("vehiculoId", vehiculoId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        foreach (var rol in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, rol));
        }

        var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opciones.ClaveSecreta));
        var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _opciones.Emisor,
            audience: _opciones.Audiencia,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: fechaExpiracion,
            signingCredentials: credenciales);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
