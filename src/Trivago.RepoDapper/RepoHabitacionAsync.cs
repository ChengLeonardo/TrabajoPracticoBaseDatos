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
    // 1️⃣ primer query: Habitacion + TipoHabitacion
    string sqlHabitacion = @"
        SELECT h.*, t.idTipo, t.Nombre
        FROM Habitacion h
        INNER JOIN TipoHabitacion t ON h.idTipo = t.idTipo
        WHERE h.idHabitacion = @Id;
    ";

    var habitacion = (await _conexion.QueryAsync<Habitacion, TipoHabitacion, Habitacion>(
        sqlHabitacion,
        (h, t) => { h.tipoHabitacion = t; return h; },
        new { Id = id },
        splitOn: "idTipo"
    )).SingleOrDefault();

    if (habitacion is null)
        return null;

    // 2️⃣ segundo query: Comentarios + Reservas
    string sqlExtras = @"
        SELECT * FROM Comentario WHERE idHabitacion = @Id;
        SELECT * FROM Reserva WHERE idHabitacion = @Id;
    ";

    using var multi = await _conexion.QueryMultipleAsync(sqlExtras, new { Id = id });

    habitacion.Comentarios = (await multi.ReadAsync<Comentario>()).ToList();
    habitacion.Reservas = (await multi.ReadAsync<Reserva>()).ToList();

    return habitacion;
}



    public async Task<List<Habitacion>> ListarAsync()
    {
        string sql = @"
            SELECT h.*, t.* 
            FROM Habitacion h
            JOIN TipoHabitacion t ON h.idTipo = t.idTipo;";

        var resultado = await _conexion.QueryAsync<Habitacion, TipoHabitacion, Habitacion>(
            sql,
            (habitacion, tipo) =>
            {
                habitacion.tipoHabitacion = tipo;
                return habitacion;
            },
            splitOn: "idTipo"
        );

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

