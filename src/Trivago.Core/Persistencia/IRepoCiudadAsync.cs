using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoCiudadAsync :  IAltaAsync<Ciudad, uint>, IListadoAsync<Ciudad>, IDetalleAsync<Ciudad, uint>
{
    Task<List<Ciudad>> InformarCiudadPorIdPaisAsync(uint idPais);
}