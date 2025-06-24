using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoComentarioTest : TestBase
{
    private readonly IRepoComentario _RepoComentario ;
    private readonly IRepoComentarioAsync _RepoComentarioAsync;

    public RepoComentarioTest() : base(){
        _RepoComentario = new RepoComentario(Conexion);
        _RepoComentarioAsync = new RepoComentarioAsync(Conexion);
    }

    [Fact]
    public void InformarComentario()
    {
        var detalle = _RepoComentario.Detalle(1);

        Assert.NotNull(detalle);
    }
    [Fact]
    public void Listar()
    {
        var lista = _RepoComentario.Listar();

        Assert.NotNull(lista);
        Assert.NotEmpty(lista);
        Assert.Contains(lista, objec => objec.comentario == "nose" && objec.idComentario == 3);
    }
    [Fact]
    public void Alta()
    {
        var comentario = new Comentario {comentario = "ja", Calificacion = 3, Fecha = new DateTime(2025, 02, 1), Habitacion = 2, idComentario = 0};
        var id = _RepoComentario.Alta(comentario);
        comentario.idComentario = id;

        Assert.NotEqual<uint>(0, id);
        Assert.NotNull(_RepoComentario.Detalle(id));
    }

    [Fact]
    public void ListarPorHabitacion()
    {
        var lista = _RepoComentario.ListarPorHabitacion(1);

        Assert.Contains(lista, comentario => comentario.Calificacion == 3);
    }

    
    [Fact]
    public async Task InformarComentarioAsync()
    {
        var detalle = await _RepoComentarioAsync.DetalleAsync(1);

        Assert.NotNull(detalle);
    }
    [Fact]
    public async Task ListarAsync()
    {
        var lista = await _RepoComentarioAsync.ListarAsync();

        Assert.NotNull(lista);
        Assert.NotEmpty(lista);
        Assert.Contains(lista, objec => objec.comentario == "nose" && objec.idComentario == 3);
    }
    [Fact]
    public async Task AltaAsync()
    {
        var comentario = new Comentario {comentario = "ja", Calificacion = 3, Fecha = new DateTime(2025, 02, 1), Habitacion = 2, idComentario = 0};
        var id = await _RepoComentarioAsync.AltaAsync(comentario);
        comentario.idComentario = id;

        Assert.NotEqual<uint>(0, id);
        Assert.NotNull(await _RepoComentarioAsync.DetalleAsync(id));
    }

    [Fact]
    public async Task ListarPorHabitacionAsync()
    {
        var lista = await _RepoComentarioAsync.ListarPorHabitacionAsync(1);

        Assert.Contains(lista, comentario => comentario.Calificacion == 3);
    }
}
