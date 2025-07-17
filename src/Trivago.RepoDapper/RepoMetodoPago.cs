using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoMetodoPago : RepoDapper, IRepoMetodoPago
{
    public RepoMetodoPago(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(MetodoPago metodoPago)
    {
        string storedProcedure = "insert_metodo_pago";

        var parametros = new DynamicParameters();
        parametros.Add("p_TipoMedioPago", metodoPago.TipoMedioPago);
        parametros.Add("p_idMetodoPago", direction: ParameterDirection.Output);
               
        _conexion.Execute(storedProcedure, parametros);

        metodoPago.idMetodoPago = parametros.Get<uint>("p_idMetodoPago");
        return metodoPago.idMetodoPago;
    }

    public MetodoPago? Detalle(uint id)
    {
        string sql = @" select * from MetodoPago
                        where idMetodoPago = @Id
                        LIMIT 1;

                        select * from Reserva
                        Where idMetododePago = @Id;
                        ";
        using ( var multi = _conexion.QueryMultiple(sql, new { Id = id }))
        {
            var MetodoPago = multi.ReadSingleOrDefault<MetodoPago>();
            if (MetodoPago != null)
            {
                MetodoPago.Reservas = multi.Read<Reserva>().ToList();
            }
            return MetodoPago;
        }
    }

    public List<MetodoPago> Listar()
    {
        string sql = "Select * from MetodoPago";
        var resultado = _conexion.Query<MetodoPago>(sql).ToList();
        return resultado;
    }
}