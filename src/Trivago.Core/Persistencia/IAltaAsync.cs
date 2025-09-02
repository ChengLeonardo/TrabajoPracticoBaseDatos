using System.Linq.Expressions;
using System.Numerics;

namespace Trivago.Core.Persistencia;

public interface IAltaAsync<T,N> where N : IBinaryNumber<N>
{
    Task<N> AltaAsync(T elemento);
}