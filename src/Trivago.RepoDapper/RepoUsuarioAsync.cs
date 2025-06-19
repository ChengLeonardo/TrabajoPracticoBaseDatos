using System.Data;
using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoUsuarioAsync : RepoDapper, IRepoUsuarioAsync
{
    public RepoUsuarioAsync(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<uint> AltaAsync(Usuario usuario)
    {
        string storedProcedure = "insert_usuario";

        var parametros = new DynamicParameters();
        parametros.Add("p_Nombre", usuario.Nombre);
        parametros.Add("p_Apellido", usuario.Apellido);
        parametros.Add("p_Mail", usuario.Mail);
        parametros.Add("p_Contraseña", usuario.Contrasena);
        parametros.Add("p_idUsuario", ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        usuario.idUsuario = parametros.Get<uint>("p_idUsuario");
        return usuario.idUsuario;
    }
    public async Task<Usuario?> DetalleAsync(uint id)
    {
        string sql = "Select * from Usuario where idUsuario = @Id LIMIT 1";
        var resultado = await _conexion.QuerySingleOrDefaultAsync<Usuario>(sql, new { Id = id});
        return resultado;
    }

    public async Task<List<Usuario>> ListarAsync()
    {
        string sql = "Select * from Usuario";
        var resultado = await _conexion.QueryAsync<Usuario>(sql);
        return resultado.ToList();
    }

    public Task<Usuario?> UsuarioPorPassAsync(string email, string pass)
    {
        throw new NotImplementedException();
    }
}