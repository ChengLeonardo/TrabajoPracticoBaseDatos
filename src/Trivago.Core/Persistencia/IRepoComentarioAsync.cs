using System.ComponentModel.Design;
using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoComentarioAsync :  IAltaAsync<Comentario, uint>, IListadoAsync<Comentario>, IDetalleAsync<Comentario, uint>
{
    Task<List<Comentario>> ListarPorHabitacionAsync(uint idHabitacion);
}