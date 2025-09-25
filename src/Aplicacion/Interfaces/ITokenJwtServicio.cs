using System;
using System.Collections.Generic;

namespace LocalizadorGps.Aplicacion.Interfaces;

public interface ITokenJwtServicio
{
    string GenerarToken(Guid dispositivoId, Guid vehiculoId, string nombreUsuario, IEnumerable<string> roles, DateTime expiracion);
}
