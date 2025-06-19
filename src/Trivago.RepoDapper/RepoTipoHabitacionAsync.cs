using System.Data;
using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoTipoHabitacionAsync : RepoDapper, IRepoTipoHabitacionAsync
{
    public RepoTipoHabitacionAsync(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<uint> AltaAsync(TipoHabitacion tipoHabitacion)
    {
        string storedProcedure = "insert_tipo_habitacion";

        var parametros = new DynamicParameters();
        parametros.Add("p_Nombre", tipoHabitacion.Nombre);
        parametros.Add("p_idTipo", ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        tipoHabitacion.idTipo= parametros.Get<uint>("p_idTipo");
        return tipoHabitacion.idTipo;
    }

    public async Task<TipoHabitacion?> DetalleAsync(uint id)
    {
        string sql = "Select * from TipoHabitacion where idTipo = @Id LIMIT 1";
        var resultado = await _conexion.QuerySingleOrDefaultAsync<TipoHabitacion>(sql, new { Id = id});
        return resultado;
    }

    public async Task<List<TipoHabitacion>> ListarAsync()
    {
        string sql = "Select * from TipoHabitacion";
        var resultado = await _conexion.QueryAsync<TipoHabitacion>(sql);
        return resultado.ToList();
    }
}