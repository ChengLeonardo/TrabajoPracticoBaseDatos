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
        parametros.Add("p_Contrasena", usuario.Contrasena);
        parametros.Add("p_idUsuario", direction: ParameterDirection.Output);
               
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

    public async Task<Usuario?> UsuarioPorPassAsync(string email, string pass)
    {
        Usuario? resultado = null;
        string sql = "Select verificacion_usuario(@mail, @Contrasena)";
        var correcto = await _conexion.QuerySingleAsync<int>(sql, new { mail = email, Contrasena = pass});
        if(correcto == 1)
        {
            sql = "Select * from Usuario where Mail = @mail";
            resultado = await _conexion.QuerySingleAsync<Usuario>(sql, new { mail = email});
        }
        return resultado;
    }
}