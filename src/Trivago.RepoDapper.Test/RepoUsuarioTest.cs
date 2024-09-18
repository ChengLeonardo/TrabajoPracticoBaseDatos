using Trivago.Core.Ubicacion;
using Xunit;

namespace Trivago.RepoDapper.Test;

public class RepoUsuarioTest : TestBase
{
    private RepoUsuario _repoUsuario;

    public RepoUsuarioTest(): base()
    {
        this._repoUsuario = new RepoUsuario(Conexion);
    }

    [Fact]
    public void Insertar()
    {
        var usuario = new Usuario
        {
            Apellido = "TestApe",
            Nombre = "TestNombre",
            Contrasena = "234234",
            idreserva = 0,
            idUsuario = 0,
            Mail = "Test@gmail.com"
        };

        var id = _repoUsuario.Alta(usuario);

        usuario.idUsuario = id;

        Assert.NotEqual<uint>(0, usuario.idUsuario);
        Assert.NotNull(usuario);
    }

    [Theory]
    [InlineData("Leonardo")]
    [InlineData("Luz")]
    [InlineData("Mario")]
    public void Listar(string nombre)
    {
        var lista = _repoUsuario.Listar();

        Assert.NotNull(lista);
        Assert.Contains(lista, usuario => usuario.Nombre == nombre);
    }

    [Fact]
    public void Detalle()
    {
        var usuario = _repoUsuario.Detalle(1);

        Assert.NotNull(usuario);
        Assert.Equal(usuario.Nombre, "Leonardo");
    }

    [Fact]
    public void VerificarUsuario()
    {
        var usuarioPaVerificar = _repoUsuario.UsuarioPorPass("leonardocheng@gmail.com", "123");

        Assert.True(usuarioPaVerificar);
    }
}
