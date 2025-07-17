using System.Data;
using System.Data.Common;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoHabitacion : RepoDapper, IRepoHabitacion
{
    public RepoHabitacion(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Habitacion habitacion)
    {
        string storedProcedure = "insert_habitacion";

        var parametros = new DynamicParameters();

        parametros.Add("p_idHotel", habitacion.hotel.idHotel);
        parametros.Add("p_idTipo", habitacion.tipoHabitacion.idTipo);
        parametros.Add("p_PrecioPorNoche", habitacion.PrecioPorNoche);
        parametros.Add("p_idHabitacion",direction: ParameterDirection.Output);
               
        _conexion.Execute(storedProcedure, parametros);

        habitacion.idHabitacion = parametros.Get<uint>("p_idHabitacion");
        return habitacion.idHabitacion;
    }


    public Habitacion? Detalle(uint id)
    {
                string sql = @" select * from Habitacion
                        where idHabitacion = @Id
                        LIMIT 1;

                        select * from Comentario
                        Where idHabitacion = @Id;

                        select * from Reserva
                        where idHabitacion = @Id;
                        ";
        using ( var multi = _conexion.QueryMultiple(sql, new { Id = id }))
        {
            var habitacion = multi.ReadSingleOrDefault<Habitacion>();
            if (habitacion != null)
            {
                habitacion.Comentarios = multi.Read<Comentario>().ToList();
                habitacion.Reservas = multi.Read<Reserva>().ToList();
            }
            return habitacion;
        }
    }

    public List<Habitacion> Listar()
    {
        string sql = "Select * from Habitacion";
        var resultado = _conexion.Query<Habitacion>(sql).ToList();
        return resultado;
    }

    public List<Habitacion> InformarHabitacionPorIdHotel(uint idHotel)
    {
        string sql = "Select * from Habitacion where idHotel = @Id";
        var resultado = _conexion.Query<Habitacion>(sql, new {Id = idHotel}).ToList();
        return resultado;
    }
    
    public List<Habitacion> InformarHabitacionPorIdTipo(uint idTipo)
    {
        string sql = "Select * from Habitacion where idTipo = @Id";
        var resultado = _conexion.Query<Habitacion>(sql, new {Id = idTipo}).ToList();
        return resultado;
    }
}

