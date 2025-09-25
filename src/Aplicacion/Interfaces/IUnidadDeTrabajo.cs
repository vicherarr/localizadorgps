using System.Threading;
using System.Threading.Tasks;

namespace LocalizadorGps.Aplicacion.Interfaces;

public interface IUnidadDeTrabajo
{
    Task<int> GuardarCambiosAsync(CancellationToken ct = default);
}
