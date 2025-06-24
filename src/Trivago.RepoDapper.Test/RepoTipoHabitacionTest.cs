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
    public void InsertarAsync()
    {
        var habitacion = new Habitacion
        {
            Comentarios = new List<Comentario>(),
            hotel = _repoHotel.Detalle(1),
            idHabitacion = 0,
            PrecioPorNoche = 89,
            tipoHabitacion = _repoTipoHabitacion.Detalle(1)
        };

        habitacion = 
    }
}
