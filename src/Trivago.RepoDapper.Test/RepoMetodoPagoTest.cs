using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoMetodoPagoTest : TestBase
{
    private RepoMetodoPago _repoMetodoPago;
        private RepoMetodoPagoAsync _repoMetodoPagoAsync;

    public RepoMetodoPagoTest() : base()
    {
        _repoMetodoPago = new RepoMetodoPago(Conexion);
        _repoMetodoPagoAsync = new RepoMetodoPagoAsync(Conexion);
    }
    [Fact]
    public void Detalle()
    {
        var detalle = _repoMetodoPago.Detalle(2);

        Assert.NotNull(detalle);
        Assert.Equal(detalle.TipoMedioPago, "Efectivo");
    }
    [Theory]
    [InlineData("Efectivo")]
    [InlineData("Mercado Pago")]
    [InlineData("VisaDebito")]
    public void InforarMetodoPago(string tipoMedioPago)
    {
        var metodoPagos = _repoMetodoPago.Listar();

        Assert.Contains(metodoPagos, metodoPago => metodoPago.TipoMedioPago == tipoMedioPago);
    }
    
}
