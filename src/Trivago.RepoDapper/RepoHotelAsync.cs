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
                        string sql = @" select * from Hotel
                        where idHotel = @Id
                        LIMIT 1;

                        select * from Habitacion
                        Where idHotel = @Id;
                        ";
        using ( var multi = await  _conexion.QueryMultipleAsync(sql, new { Id = id }))
        {
            var hotel = await  multi.ReadSingleOrDefaultAsync<Hotel>();
            if (hotel != null)
            {
                var Habitaciones = await  multi.ReadAsync<Habitacion>();
                hotel.Habitaciones = Habitaciones.ToList();
            }
            return hotel;
        }
    }

    public async Task<List<Hotel>> ListarAsync()
    {
        string sql = "Select * from Hotel";
        var resultado = await _conexion.QueryAsync<Hotel>(sql);
        return resultado.ToList();
    }

}