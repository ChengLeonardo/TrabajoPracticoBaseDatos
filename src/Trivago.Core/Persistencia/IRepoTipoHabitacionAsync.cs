using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoTipoHabitacionAsync :  IAltaAsync<TipoHabitacion, uint>, IListadoAsync<TipoHabitacion>, IDetalleAsync<TipoHabitacion, uint>
{
    
}