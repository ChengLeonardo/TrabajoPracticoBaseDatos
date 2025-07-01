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

    public RepoTipoHabitacionTest() : base()
    {
        _repoHabitacion = new RepoHabitacion(Conexion);
        _repoComentario = new RepoComentario(Conexion);
        _repoHotel = new RepoHotel(Conexion);
        _repoTipoHabitacion = new RepoTipoHabitacion(Conexion);
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

    //aca faltan
}
