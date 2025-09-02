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

    [Fact]
    public void Insertar()
    {
        var tipoMedioPago = new MetodoPago{
            TipoMedioPago = "testUnit"
        
        };

        var idAutoIncrement = _repoMetodoPago.Alta(tipoMedioPago);

        var listaMetodoPago = _repoMetodoPago.Listar();
        
        Assert.Equal("5", $"{idAutoIncrement}");
        Assert.NotNull(_repoMetodoPago.Detalle(idAutoIncrement));
        Assert.Contains(listaMetodoPago, metodoPago => metodoPago.idMetodoPago == idAutoIncrement);
    }
    [Fact]
        public async Task DetalleAsync()
    {
        var detalle = await _repoMetodoPagoAsync.DetalleAsync(2);

        Assert.NotNull(detalle);
        Assert.Equal(detalle.TipoMedioPago, "Efectivo");
    }
    [Theory]
    [InlineData("Efectivo")]
    [InlineData("Mercado Pago")]
    [InlineData("VisaDebito")]
    public async Task InforarMetodoPagoAsync(string tipoMedioPago)
    {
        var metodoPagos = await _repoMetodoPagoAsync.ListarAsync();

        Assert.Contains(metodoPagos, metodoPago => metodoPago.TipoMedioPago == tipoMedioPago);
    }

    [Fact]
    public async Task InsertarAsync()
    {
        var tipoMedioPago = new MetodoPago{
            TipoMedioPago = "testUnitAsync"
        
        };

        var idAutoIncrement = await _repoMetodoPagoAsync.AltaAsync(tipoMedioPago);

        var listaMetodoPago = await _repoMetodoPagoAsync.ListarAsync();
        
        Assert.Equal("4", $"{idAutoIncrement}");
        Assert.NotNull(await _repoMetodoPagoAsync.DetalleAsync(idAutoIncrement));
        Assert.Contains(listaMetodoPago, metodoPago => metodoPago.idMetodoPago == idAutoIncrement);
    }
}