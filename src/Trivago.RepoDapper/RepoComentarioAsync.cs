using System.Threading.Tasks;

namespace Trivago.RepoDapper;
public class RepoComentarioAsync : RepoDapper, IRepoComentarioAsync
{
    public RepoComentarioAsync(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<uint> AltaAsync(Comentario comentario)
    {
        string storedProcedure = "insert_comentario";

        var parametros = new DynamicParameters();
        parametros.Add("p_idHabitacion", comentario.Habitacion);
        parametros.Add("p_Comentario", comentario.comentario);
        parametros.Add("p_Calificacion", comentario.Calificacion);
        parametros.Add("p_idComentario", direction: ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        comentario.idComentario = parametros.Get<uint>("p_idComentario");
        return comentario.idComentario;
    }


    public async Task<Comentario?> DetalleAsync(uint id)
    {
        string sql = "Select * from Comentario where idComentario = @Id LIMIT 1";
        var resultado = await _conexion.QuerySingleOrDefaultAsync<Comentario>(sql, new { Id = id});
        return resultado;
    }

    public async Task<List<Comentario>> ListarAsync()
    {
        string sql = "Select * from Comentario";
        var resultado = await _conexion.QueryAsync<Comentario>(sql);
        return resultado.ToList();
    }

    public async Task<List<Comentario>> ListarPorIdHabitacionAsync(uint idHabitacion)
    {
        string sql = "Select * from Comentario where idHabitacion = @IdHabitacion";
        var resultado = await _conexion.QueryAsync<Comentario>(sql, new { idHabitacion = idHabitacion} );

        return resultado.ToList();
    }
}