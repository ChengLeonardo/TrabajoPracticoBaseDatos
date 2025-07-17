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
        parametros.Add("p_idTipo", direction: ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        tipoHabitacion.idTipo= parametros.Get<uint>("p_idTipo");
        return tipoHabitacion.idTipo;
    }

    public async Task<TipoHabitacion?> DetalleAsync(uint id)
    {
        string sql = @" select * from TipoHabitacion
                        where idTipo = @Id
                        LIMIT 1;

                        select * from Habitacion
                        Where idTipo = @Id;
                        ";
        using ( var multi = await _conexion.QueryMultipleAsync(sql, new { Id = id }))
        {
            var TipoHabitacion = await multi.ReadSingleOrDefaultAsync<TipoHabitacion>();
            if (TipoHabitacion != null)
            {
                var Habitaciones = await multi.ReadAsync<Habitacion>();
                TipoHabitacion.Habitaciones = Habitaciones.ToList();
            }
            return TipoHabitacion;
        }
    }

    public async Task<List<TipoHabitacion>> ListarAsync()
    {
        string sql = "Select * from TipoHabitacion";
        var resultado = await _conexion.QueryAsync<TipoHabitacion>(sql);
        return resultado.ToList();
    }
}