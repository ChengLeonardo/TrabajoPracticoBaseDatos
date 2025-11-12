using System.Data;
using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoHotelAsync : RepoDapper, IRepoHotelAsync
{
    public RepoHotelAsync(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<List<Hotel>> InformarHotelesPorIdCiudadAsync(int idCiudad)
    {
        string sql = "Select * from Hotel where idCiudad = @Id";
        var resultado = await  _conexion.QueryAsync <Hotel>(sql, new { Id = idCiudad});
        return resultado.ToList();
    }
    public async Task<uint> AltaAsync(Hotel hotel)
    {
        string storedProcedure = "insert_hotel";

        var parametros = new DynamicParameters();
        parametros.Add("p_idCiudad", hotel.idCiudad);
        parametros.Add("p_Nombre", hotel.Nombre);
        parametros.Add("p_Direccion", hotel.Direccion);
        parametros.Add("p_Telefono", hotel.Telefono);
        parametros.Add("p_URL", hotel.URL);
        parametros.Add("p_idHotel",direction: ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        hotel.idHotel = parametros.Get<uint>("p_idHotel");
        return hotel.idHotel;
    }


public async Task<Hotel?> DetalleAsync(uint id)
{
    string sql = @"
        SELECT 
            ho.idHotel, ho.Nombre, ho.Direccion, ho.Telefono, ho.URL,
            h.idHabitacion, h.idTipo, h.nroHabitacion, h.PrecioPorNoche,
            t.idTipo, t.Nombre AS NombreTipo
        FROM Hotel ho
        INNER JOIN Habitacion h ON ho.idHotel = h.idHotel
        INNER JOIN TipoHabitacion t ON h.idTipo = t.idTipo
        WHERE ho.idHotel = @Id;
    ";

    var hotelDict = new Dictionary<uint, Hotel>();

    var resultado = await _conexion.QueryAsync<Hotel, Habitacion, TipoHabitacion, Hotel>(
        sql,
        (ho, hab, tipo) =>
        {
            if (!hotelDict.TryGetValue(ho.idHotel, out var hotelEntry))
            {
                hotelEntry = ho;
                hotelEntry.Habitaciones = new List<Habitacion>();
                hotelDict.Add(hotelEntry.idHotel, hotelEntry);
            }

            // Asigna el tipo a la habitación
            hab.tipoHabitacion = tipo;

            // Agrega la habitación si no estaba
            hotelEntry.Habitaciones.Add(hab);

            return hotelEntry;
        },
        new { Id = id },
        splitOn: "idHabitacion,idTipo"
    );

    return hotelDict.Values.SingleOrDefault();
}

    public async Task<List<Hotel>> ListarAsync()
    {
        string sql = "Select * from Hotel";
        var resultado = await _conexion.QueryAsync<Hotel>(sql);
        return resultado.ToList();
    }

}