using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoReservaAsync :  IAltaAsync<Reserva, uint>, IListadoAsync<Reserva>, IDetalleAsync<Reserva, uint>
{
    Task<List<Reserva>> InformarReservasPorIdHabitacionAsync(uint idHabitacion);
    Task<List<Reserva>> InformarReservasPorIdMetodoPagoAsync(uint idMetodoPago);
}