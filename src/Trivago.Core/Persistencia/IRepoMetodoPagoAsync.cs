using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoMetodoPagoAsync :  IAltaAsync<MetodoPago, uint>, IListadoAsync<MetodoPago>, IDetalleAsync<MetodoPago, uint>
{
    
}