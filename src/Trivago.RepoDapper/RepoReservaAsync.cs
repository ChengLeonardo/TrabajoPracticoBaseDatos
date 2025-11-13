using System.Data;
using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoReservaAsync : RepoDapper, IRepoReservaAsync
{
    public RepoReservaAsync(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<uint> AltaAsync(Reserva reserva)
    {
        string storedProcedure = "insert_reserva";

        var parametros = new DynamicParameters();
        parametros.Add("p_idHabitacion", reserva.habitacion.idHabitacion);
        parametros.Add("p_idMetodoPago", reserva.metodoPago.idMetodoPago);
        parametros.Add("p_idUsuario", reserva.idUsuario);
        parametros.Add("p_Entrada", reserva.Entrada);
        parametros.Add("p_Salida", reserva.Salida);
        parametros.Add("p_Telefono", reserva.Telefono);
        parametros.Add("p_idReserva", direction: ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        reserva.idReserva = parametros.Get<uint>("p_idReserva");
        return reserva.idReserva;
    }

    public async Task<Reserva?> DetalleAsync(uint id)
    {
        string sql = @"
            SELECT 
                -- columnas de Reserva
                r.*,
                
                -- columnas de Habitacion
                h.*,
                
                -- columnas de TipoHabitacion
                t.*,
                
                -- columnas de MetodoPago
                m.*,
                u.*,

                ho.*
            FROM Reserva r
            INNER JOIN Habitacion h ON r.idHabitacion = h.idHabitacion
            INNER JOIN TipoHabitacion t ON h.idTipo = t.idTipo
            INNER JOIN MetodoPago m ON r.idMetodoPago = m.idMetodoPago
            inner join Usuario u on u.idUsuario = r.idUsuario
            inner join Hotel ho on ho.idHotel = h.idHotel
            WHERE r.idReserva = @Id;
        ";
        var resultado = await _conexion.QueryAsync<Reserva, Habitacion, TipoHabitacion, MetodoPago, Usuario, Hotel, Reserva>(
            sql,
            (r, h, t, m, u, ho) =>
            {
                h.tipoHabitacion = t;
                h.hotel = ho;
                r.habitacion = h;
                r.metodoPago = m;
                r.usuario = u;
                return r;
            },
            new { Id = id },
            splitOn: "idHabitacion,idTipo,idMetodoPago,idUsuario,idHotel" // 👈 debe coincidir EXACTAMENTE con las columnas del SELECT
        );
        return resultado.SingleOrDefault();
    }


    public async Task<List<Reserva>> ListarAsync()
    {
                string sql = @"
            SELECT 
                -- columnas de Reserva
                r.*,
                h.*,
                ho.*
            FROM Reserva r
            INNER JOIN Habitacion h ON r.idHabitacion = h.idHabitacion
            inner join Hotel ho on ho.idHotel = h.idHotel;
        ";
        var resultado = await _conexion.QueryAsync<Reserva, Habitacion, Hotel, Reserva>(
            sql,
            (r, h, ho) =>
            {
                h.hotel = ho;
                r.habitacion = h;
                return r;
            },
            splitOn: "idHabitacion,idHotel" 
        );
        return resultado.ToList();
    }

        public async Task<List<Reserva>> InformarReservasPorIdHabitacionAsync(uint idHabitacion)
    {
        string sql = "Select * from Reserva where idHabitacion = @Id";
        var resultado = await _conexion.QueryAsync<Reserva>(sql, new { Id = idHabitacion});
        return resultado.ToList();
    }

    public async Task<List<Reserva>> InformarReservasPorIdMetodoPagoAsync(uint idMetodoPago)
    {
        string sql = "Select * from Reserva where idMetodoPago = @Id";
        var resultado = await  _conexion.QueryAsync<Reserva>(sql, new { Id = idMetodoPago});
        return resultado.ToList();
    }
}