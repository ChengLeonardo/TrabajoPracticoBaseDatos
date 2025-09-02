using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoHabitacionAsync :  IAltaAsync<Habitacion, uint>, IListadoAsync<Habitacion>, IDetalleAsync<Habitacion, uint>
{
    Task<List<Habitacion>> InformarHabitacionPorIdHotelAsync(uint idHotel);
    Task<List<Habitacion>> InformarHabitacionPorIdTipoAsync(uint idTipo);
}