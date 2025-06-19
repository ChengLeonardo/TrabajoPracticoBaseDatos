using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoHabitacionAsync :  IAltaAsync<Habitacion, uint>, IListadoAsync<Habitacion>, IDetalleAsync<Habitacion, uint>
{
    
}