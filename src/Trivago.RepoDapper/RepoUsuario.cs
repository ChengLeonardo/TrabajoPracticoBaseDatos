using System.Data;
using System.Net.Mail;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoUsuario : RepoDapper, IRepoUsuario
{
    public RepoUsuario(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Usuario usuario)
    {
        string storedProcedure = "insert_usuario";

        var parametros = new DynamicParameters();
        parametros.Add("p_Nombre", usuario.Nombre);
        parametros.Add("p_Apellido", usuario.Apellido);
        parametros.Add("p_Mail", usuario.Mail);
        parametros.Add("p_Contrasena", usuario.Contrasena);
        parametros.Add("p_idUsuario", direction: ParameterDirection.Output);
               
        _conexion.Execute(storedProcedure, parametros);

        usuario.idUsuario = parametros.Get<uint>("p_idUsuario");
        return usuario.idUsuario;
    }
    public Usuario? Detalle(uint id)
    {
        string sql = "Select * from Usuario where idUsuario = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<Usuario>(sql, new { Id = id});
        return resultado;
    }

    public List<Usuario> Listar()
    {
        string sql = "Select * from Usuario";
        var resultado = _conexion.Query<Usuario>(sql).ToList();
        return resultado;
    }

    public Usuario? UsuarioPorPass(string email, string pass)
    {
        Usuario? resultado = null;
        string sql = "Select verificacion_usuario(@mail, @Contrasena)";
        var correcto = _conexion.QuerySingle<int>(sql, new { mail = email, Contrasena = pass});
        if(correcto == 1)
        {
            sql = "Select * from Usuario where Mail = @mail";
            resultado = _conexion.QuerySingle<Usuario>(sql, new { mail = email});
        }
        return resultado;
    }
}