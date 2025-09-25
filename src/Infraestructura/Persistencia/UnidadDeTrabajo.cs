using System.Threading;
using System.Threading.Tasks;
using LocalizadorGps.Aplicacion.Interfaces;

namespace LocalizadorGps.Infraestructura.Persistencia;

internal class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly LocalizadorGpsDbContext _contexto;

    public UnidadDeTrabajo(LocalizadorGpsDbContext contexto)
    {
        _contexto = contexto;
    }

    public Task<int> GuardarCambiosAsync(CancellationToken ct = default) => _contexto.SaveChangesAsync(ct);
}
