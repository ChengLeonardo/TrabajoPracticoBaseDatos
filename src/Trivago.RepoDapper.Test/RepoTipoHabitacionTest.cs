using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Xunit;

namespace Trivago.RepoDapper.Test;

public class RepoTipoHabitacionTest : TestBase
{
    private IRepoHabitacion _repoHabitacion;
    private IRepoComentario _repoComentario;
    private IRepoHotel _repoHotel;
    private RepoTipoHabitacion _repoTipoHabitacion;
        private IRepoHabitacionAsync _repoHabitacionAsync;
    private IRepoComentarioAsync _repoComentarioAsync;
    private IRepoHotelAsync _repoHotelAsync;
    private RepoTipoHabitacionAsync _repoTipoHabitacionAsync;
    public RepoTipoHabitacionTest() : base()
    {
        _repoHabitacion = new RepoHabitacion(Conexion);
        _repoComentario = new RepoComentario(Conexion);
        _repoHotel = new RepoHotel(Conexion);
        _repoTipoHabitacion = new RepoTipoHabitacion(Conexion);
        _repoHabitacionAsync = new RepoHabitacionAsync(Conexion);
        _repoComentarioAsync = new RepoComentarioAsync(Conexion);
        _repoHotelAsync = new RepoHotelAsync(Conexion);
        _repoTipoHabitacionAsync = new RepoTipoHabitacionAsync(Conexion);
    }

    [Fact]
    public void Insertar()
    {
        var testtipoHabitacion = new TipoHabitacion()
        {
            Nombre = "test"
        };

        testtipoHabitacion.idTipo = _repoTipoHabitacion.Alta(testtipoHabitacion);
        Assert.NotNull(_repoTipoHabitacion.Detalle(testtipoHabitacion.idTipo));
        Assert.Contains(_repoTipoHabitacion.Listar(), tipoHabitacion => tipoHabitacion.Nombre == testtipoHabitacion.Nombre);

    }
    [Theory]
    [InlineData("Suite")]
    [InlineData("Junior suite")]
    [InlineData("Gran suite")]
    public void Listar(string nombreTipoHabitacion)
    {
        var lista = _repoTipoHabitacion.Listar();

        Assert.NotNull(lista);
        Assert.Contains(lista, tipoHabitacion => tipoHabitacion.Nombre == nombreTipoHabitacion);
    }

    [Theory]
    [InlineData("Suite", 1)]
    [InlineData("Junior suite", 2)]
    [InlineData("Gran suite", 3)]
    public void Detalle(string nombreTipoHabitacion, uint id)
    {
        var tipoHabitacion = _repoTipoHabitacion.Detalle(id);

        Assert.NotNull(tipoHabitacion);
        Assert.Equal(nombreTipoHabitacion, tipoHabitacion.Nombre);
    }

        [Fact]
    public async Task InsertarAsync()
    {
        var testtipoHabitacion = new TipoHabitacion()
        {
            Nombre = "testAsync"
        };

        testtipoHabitacion.idTipo = await _repoTipoHabitacionAsync.AltaAsync(testtipoHabitacion);
        Assert.NotNull(await _repoTipoHabitacionAsync.DetalleAsync(testtipoHabitacion.idTipo));
        Assert.Contains(await _repoTipoHabitacionAsync.ListarAsync(), tipoHabitacion => tipoHabitacion.Nombre == testtipoHabitacion.Nombre);

    }
    [Theory]
    [InlineData("Suite")]
    [InlineData("Junior suite")]
    [InlineData("Gran suite")]
    public async Task ListarAsync(string nombreTipoHabitacion)
    {
        var lista = await _repoTipoHabitacionAsync.ListarAsync();

        Assert.NotNull(lista);
        Assert.Contains(lista, tipoHabitacion => tipoHabitacion.Nombre == nombreTipoHabitacion);
    }

    [Theory]
    [InlineData("Suite", 1)]
    [InlineData("Junior suite", 2)]
    [InlineData("Gran suite", 3)]
    public async Task DetalleAsync(string nombreTipoHabitacion, uint id)
    {
        var tipoHabitacion = await _repoTipoHabitacionAsync.DetalleAsync(id);

        Assert.NotNull(tipoHabitacion);
        Assert.Equal(nombreTipoHabitacion, tipoHabitacion.Nombre);
    }

}
