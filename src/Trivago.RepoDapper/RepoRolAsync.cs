using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trivago.RepoDapper;

public class RepoRolAsync : RepoDapper, IRepoRolAsync
{

    public RepoRolAsync(IDbConnection conexion) : base(conexion)
    {
    }
        public async Task<uint> AltaAsync(Rol rol)
        {
            string storedProcedure = "insert_rol";

            var parametros = new DynamicParameters();
            parametros.Add("p_Nombre", rol.Nombre);
            parametros.Add("p_idRol", direction: ParameterDirection.Output);

            await _conexion.ExecuteAsync(storedProcedure, parametros);

            rol.idRol = parametros.Get<uint>("p_idRol");
            return rol.idRol;
        }

    public async Task<Rol?> DetalleAsync(uint id)
    {
        string sql = @"
                SELECT 
                    -- columnas de Rol
                    r.idRol, r.Nombre,
                    
                    -- columnas de Usuario
                    u.idUsuario, u.Nombre
                FROM Rol r
                INNER JOIN Usuario u ON r.idRol = u.idRol
                WHERE r.idRol = @Id;
            ";

        var resultado = await _conexion.QueryAsync<Rol, List<Usuario>, Rol>(
            sql,
            (r, u) =>
            {
                r.Usuarios = u;
                return r;
            },
            new { Id = id },
            splitOn: "idUsuario" // 👈 debe coincidir EXACTAMENTE con las columnas del SELECT
        );

        return resultado.SingleOrDefault();
    }


    public async Task<List<Rol>> ListarAsync()
    {
        string sql = "Select * from Rol";
        var resultado = await _conexion.QueryAsync<Rol>(sql);
        return resultado.ToList();
    }

}