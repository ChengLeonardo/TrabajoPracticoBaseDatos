using System.Threading.Tasks;

namespace Trivago.RepoDapper;
public class RepoCiudadAsync : RepoDapper, IRepoCiudadAsync
{
    public RepoCiudadAsync(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<uint> AltaAsync(Ciudad ciudad)
    {
        string storedProcedure = "insert_ciudad";

        var parametros = new DynamicParameters();
        parametros.Add("p_nombre", ciudad.nombre);
        parametros.Add("p_idPais", ciudad.idPais);
        parametros.Add("p_idCiudad", direction: ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        ciudad.idCiudad = parametros.Get<uint>("p_idCiudad");
        return ciudad.idCiudad;
    }

    public async Task<Ciudad>? DetalleAsync(uint id)
    {
        string sql = @" select * from Ciudad
                        where idCiudad = @Id
                        LIMIT 1;

                        select * from Hotel
                        Where idCiudad = @Id;
                        ";
        using ( var multi = await _conexion.QueryMultipleAsync(sql, new { Id = id }))
        {
            var ciudad = await multi.ReadSingleOrDefaultAsync<Ciudad>();
            if (ciudad != null)
            {
                var Hoteles = await multi.ReadAsync<Hotel>();
                ciudad.Hoteles = Hoteles.ToList();
            }
            return ciudad;
        }
    }

    public async Task<List<Ciudad>> ListarAsync()
    {
        string sql = "Select * from Ciudad";
        var resultado = await _conexion.QueryAsync<Ciudad>(sql);
        return resultado.ToList();
    }
    public async Task<List<Ciudad>> InformarCiudadPorIdPaisAsync(uint idPais)
    {
        string sql = "Select * from Ciudad where idPais = @IdPais";
        var resultado = await _conexion.QueryAsync<Ciudad>(sql, new { IdPais = idPais} );
        return resultado.ToList();
    }

}