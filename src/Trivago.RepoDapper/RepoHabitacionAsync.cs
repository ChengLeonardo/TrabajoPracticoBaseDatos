using System.Data;
using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoHabitacionAsync : RepoDapper, IRepoHabitacionAsync 
{
    public RepoHabitacionAsync(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<uint> AltaAsync(Habitacion habitacion)
    {
        string storedProcedure = "insert_habitacion";

        var parametros = new DynamicParameters();

        parametros.Add("p_idHotel", habitacion.hotel.idHotel);
        parametros.Add("p_idTipo", habitacion.tipoHabitacion.idTipo);
        parametros.Add("p_PrecioPorNoche", habitacion.PrecioPorNoche);
        parametros.Add("p_idHabitacion",direction: ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        habitacion.idHabitacion = parametros.Get<uint>("p_idHabitacion");
        return habitacion.idHabitacion;
    }


    public async Task<Habitacion?> DetalleAsync(uint id)
    {
        string sql = "Select * from Habitacion where idHabitacion = @Id LIMIT 1";
        var resultado = await _conexion.QuerySingleOrDefaultAsync<Habitacion>(sql, new { Id = id});
        return resultado;
    }

    public async Task<List<Habitacion>> ListarAsync()
    {
        string sql = "Select * from Habitacion";
        var resultado = await _conexion.QueryAsync<Habitacion>(sql);
        return resultado.ToList();
    }

        public async Task<List<Habitacion>> InformarHabitacionPorIdHotelAsync(uint idHotel)
    {
        string sql = "Select * from Habitacion where idHotel = @Id";
        var resultado = await  _conexion.QueryAsync<Habitacion>(sql, new {Id = idHotel});
        return resultado.ToList();
    }
    
    public async Task<List<Habitacion>> InformarHabitacionPorIdTipoAsync(uint idTipo)
    {
        string sql = "Select * from Habitacion where idTipo = @Id";
        var resultado = await  _conexion.QueryAsync<Habitacion>(sql, new {Id = idTipo});
        return resultado.ToList();
    }
}

