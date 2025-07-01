using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoHotelAsync :  IAltaAsync<Hotel, uint>, IListadoAsync<Hotel>, IDetalleAsync<Hotel, uint>
{
    Task<List<Hotel>> InformarHotelesPorIdCiudadAsync(int idCiudad);
}