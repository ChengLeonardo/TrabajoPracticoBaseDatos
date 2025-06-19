using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoPaisAsync : IAltaAsync<Pais, uint>, IListadoAsync<Pais>, IDetalleAsync<Pais, uint>
{
    Task<Pais>? DetallePorNombreAsync(string nombrePais);
}