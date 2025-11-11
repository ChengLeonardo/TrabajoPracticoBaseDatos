using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia
{
    public interface IRepoRolAsync : IAltaAsync<Rol, uint>, IListadoAsync<Rol>, IDetalleAsync<Rol, uint>
    {
        
    }
}