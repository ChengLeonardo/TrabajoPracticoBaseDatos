using System.Data;
using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoMetodoPagoAsync : RepoDapper, IRepoMetodoPagoAsync
{
    public RepoMetodoPagoAsync(IDbConnection conexion) : base(conexion)
    {
    }

    public async Task<uint> AltaAsync(MetodoPago metodoPago)
    {
        string storedProcedure = "insert_metodo_pago";

        var parametros = new DynamicParameters();
        parametros.Add("p_TipoMedioPago", metodoPago.TipoMedioPago);
        parametros.Add("p_idMetodoPago", ParameterDirection.Output);
               
        await _conexion.ExecuteAsync(storedProcedure, parametros);

        metodoPago.idMetodoPago = parametros.Get<uint>("p_idMetodoPago");
        return metodoPago.idMetodoPago;
    }

    public async Task<MetodoPago?> DetalleAsync(uint id)
    {
        string sql = "Select * from MetodoPago where idMetodoPago = @Id LIMIT 1";
        var resultado = await _conexion.QuerySingleOrDefaultAsync<MetodoPago>(sql, new { Id = id});
        return resultado;
    }

    public async Task<List<MetodoPago>> ListarAsync()
    {
        string sql = "Select * from MetodoPago";
        var resultado = await _conexion.QueryAsync<MetodoPago>(sql);
        return resultado.ToList();
    }
}