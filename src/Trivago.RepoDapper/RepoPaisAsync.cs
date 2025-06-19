using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Trivago.RepoDapper;
public class RepoPaisAsync : RepoDapper, IRepoPaisAsync
{
    public RepoPaisAsync(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<uint> AltaAsync(Pais pais)
    {
        string storedProcedure = "insert_pais";

        var parametros = new DynamicParameters();
        parametros.Add("p_Nombre", pais.Nombre);
        parametros.Add("p_idPais", direction: ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        pais.idPais = parametros.Get<uint>("p_idPais");
        return pais.idPais;
    }

    public async Task<Pais?> DetalleAsync(uint id)
    {
        string sql = "Select * from Pais where idPais = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefaultAsync<Pais>(sql, new { Id = id});
        return await resultado;
    }
    public async Task<Pais?> DetallePorNombreAsync(string nombrePais)
    {
        string sql = "Select * from Pais where Nombre = @Nombre Limit 1";
        var resultado = await _conexion.QuerySingleOrDefaultAsync<Pais>(sql, new {Nombre = nombrePais});
        return resultado;
    }

    public async Task<List<Pais>> ListarAsync()
    {
        string sql = "Select * from Pais";
        var resultado = await _conexion.QueryAsync<Pais>(sql);
        return resultado.ToList();
    }
}
