using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoReservaAsync :  IAltaAsync<Reserva, uint>, IListadoAsync<Reserva>, IDetalleAsync<Reserva, uint>
{
    
}