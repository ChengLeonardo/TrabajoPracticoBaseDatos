using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Xunit;

namespace Trivago.RepoDapper.Test;

public class RepoTipoHabitacionTest : TestBase
{
    private IRepoTipoHabitacion _repoTipoHabitacion;

    public RepoTipoHabitacionTest() : base()
    {
        _repoTipoHabitacion = new RepoTipoHabitacion(Conexion);
    }

    [Fact]
    public void Insertar()
    {
        var tipohabitacion = new TipoHabitacion
        {
            idTipo = 0,
            Nombre = "Test"
        };

        var id = _repoTipoHabitacion.Alta(tipohabitacion);
        Assert.NotEqual<uint>(0, id);

        tipohabitacion.idTipo = id;

        Assert.NotNull(_repoTipoHabitacion.Detalle(id));
    }

    [Theory]
    [InlineData("Suite")]
    [InlineData("Junior suite")]
    [InlineData("Gran suite")]
    public void Listar(string nombre)
    {
        var lista = _repoTipoHabitacion.Listar();

        Assert.NotNull(lista);
        Assert.Contains(lista, habitacion => habitacion.Nombre == nombre);
    }

    [Fact]
    public void Detalle()
    {
        var habitacion = _repoTipoHabitacion.Detalle(1);

        Assert.NotNull(habitacion);
        Assert.Equal(habitacion.Nombre, "Suite");
    }
}
