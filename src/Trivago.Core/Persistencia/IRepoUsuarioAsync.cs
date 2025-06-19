using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoUsuarioAsync : IAltaAsync<Usuario, uint>, IListadoAsync<Usuario>, IDetalleAsync<Usuario, uint>
{
    Task<Usuario>? UsuarioPorPassAsync(string email, string pass);
}