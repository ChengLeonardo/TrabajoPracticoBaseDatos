using System.Threading.Tasks;
using Trivago.Core.Ubicacion;
using Xunit;
using Xunit.Sdk;

namespace Trivago.RepoDapper.Test;

public class RepoUsuarioTest : TestBase
{
    private RepoUsuario _repoUsuario;
    private RepoUsuarioAsync _repoUsuarioAsync;
    public RepoUsuarioTest() : base()
    {
        _repoUsuario = new RepoUsuario(Conexion);
        _repoUsuarioAsync = new RepoUsuarioAsync(Conexion);
    }

    [Fact]
    public void Insertar()
    {
        var usuario = new Usuario
        {
            Nombre = "leo",
            Apellido = "Cheng",
            Mail = "leonardo@gmail.com",
            Contrasena = "123123",
        };

        usuario.idUsuario = _repoUsuario.Alta(usuario);
        Assert.NotNull(_repoUsuario.Detalle(usuario.idUsuario));
        Assert.Contains(_repoUsuario.Listar(), usuario => usuario.Apellido == "Cheng");
    }

    [Theory]
    [InlineData("Leonardo", 1)]
    [InlineData("Mario", 2)]
    [InlineData("Luz", 3)]
    public void Detalle(string nombre, uint id)
    {
        var usuario = _repoUsuario.Detalle(id);

        Assert.NotNull(usuario);
        Assert.Equal(nombre, usuario.Nombre);
    }

    [Theory]
    [InlineData("Leonardo")]
    [InlineData("Mario")]
    [InlineData("Luz")]
    public void Listar(string nombre)
    {
        var usuarios = _repoUsuario.Listar();

        Assert.NotNull(usuarios);
        Assert.Contains(usuarios, usuario => usuario.Nombre == nombre);
    }

        [Theory]
    [InlineData("leonardocheng@gmail.com", "123")]
    [InlineData("mariorojas@gmail.com", "321")]
    [InlineData("luzibarra@gmail.com", "231")]
    public void UsuarioPorPass(string email, string pass)
    {
        var usuario = _repoUsuario.UsuarioPorPass(email, pass);

        Assert.NotNull(usuario);
        Assert.Equal(email, usuario.Mail);
    }


    [Fact]
    public async Task InsertarAsync()
    {
        var usuario = new Usuario
        {
            Nombre = "leo",
            Apellido = "Cheng",
            Mail = "leonardoAsync@gmail.com",
            Contrasena = "123123",
        };

        usuario.idUsuario = await _repoUsuarioAsync.AltaAsync(usuario);
        Assert.NotNull(await _repoUsuarioAsync.DetalleAsync(usuario.idUsuario));
        Assert.Contains(await _repoUsuarioAsync.ListarAsync(), usuario => usuario.Apellido == "Cheng");
    }

    [Theory]
    [InlineData("Leonardo", 1)]
    [InlineData("Mario", 2)]
    [InlineData("Luz", 3)]
    public async Task DetalleAsync(string nombre, uint id)
    {
        var usuario = await _repoUsuarioAsync.DetalleAsync(id);

        Assert.NotNull(usuario);
        Assert.Equal(nombre, usuario.Nombre);
    }

    [Theory]
    [InlineData("Leonardo")]
    [InlineData("Mario")]
    [InlineData("Luz")]
    public async Task ListarAsync(string nombre)
    {
        var usuarios = await _repoUsuarioAsync.ListarAsync();

        Assert.NotNull(usuarios);
        Assert.Contains(usuarios, usuario => usuario.Nombre == nombre);
    }

        [Theory]
    [InlineData("leonardocheng@gmail.com", "123")]
    [InlineData("mariorojas@gmail.com", "321")]
    [InlineData("luzibarra@gmail.com", "231")]
    public async Task UsuarioPorPassAsync(string email, string pass)
    {
        var usuario = await _repoUsuarioAsync.UsuarioPorPassAsync(email, pass);

        Assert.NotNull(usuario);
        Assert.Equal(email, usuario.Mail);
    }
}
