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
        parametros.Add("p_nombre", ciudad.Nombre);
        parametros.Add("p_idPais", ciudad.idPais);
        parametros.Add("p_idCiudad", direction: ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        ciudad.idCiudad = parametros.Get<uint>("p_idCiudad");
        return ciudad.idCiudad;
    }

    public async Task<Ciudad>? DetalleAsync(uint id)
    {
        string sql = "Select * from Ciudad where idCiudad = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefaultAsync<Ciudad>(sql, new { Id = id});
        return await resultado;
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