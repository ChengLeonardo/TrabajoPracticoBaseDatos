namespace Trivago.Core.Persistencia;

public interface IListadoAsync<T>
{
    Task<List<T>> ListarAsync();
}