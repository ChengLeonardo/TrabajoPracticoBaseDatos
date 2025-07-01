using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoHabitacionTest : TestBase  
{
    private readonly IRepoHabitacion _repoHabitacion;
    private readonly IRepoHotel _repoHotel;
    private readonly IRepoComentario _repoComentario;
    private readonly IRepoTipoHabitacion _repoTipoHabitacion;

        private readonly IRepoHabitacionAsync _repoHabitacionAsync;
    private readonly IRepoHotelAsync _repoHotelAsync;
    private readonly IRepoComentarioAsync _repoComentarioAsync;
    private readonly IRepoTipoHabitacionAsync _repoTipoHabitacionAsync;

    public RepoHabitacionTest() : base()
    {
        _repoHabitacion = new RepoHabitacion(Conexion);
        _repoHotel = new RepoHotel(Conexion);
        _repoComentario = new RepoComentario(Conexion);
        _repoTipoHabitacion = new RepoTipoHabitacion(Conexion);
        _repoHabitacionAsync = new RepoHabitacionAsync(Conexion);
        _repoHotelAsync = new RepoHotelAsync(Conexion);
        _repoComentarioAsync = new RepoComentarioAsync(Conexion);
        _repoTipoHabitacionAsync = new RepoTipoHabitacionAsync(Conexion);
    }
    [Fact]
    public void InformarHabitacionPorId()
    {
        var detalle = _repoHabitacion.Detalle(1);

        Assert.NotNull(detalle);
        Assert.Equal(detalle.PrecioPorNoche, 10000);

    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Listar(uint idHabitacion)
    {
        var habitaciones = _repoHabitacion.Listar();

        Assert.NotEmpty(habitaciones);
        
        Assert.Contains(habitaciones, habitacion => habitacion.idHabitacion == idHabitacion);
    }
    [Fact]
     public void Insertar()
    {
        var tipoHabitacion = _repoTipoHabitacion.Detalle(1);
        var hotel = _repoHotel.Detalle(2);
        Habitacion Habitacion = new Habitacion
        {
            tipoHabitacion = tipoHabitacion,
            hotel = hotel,
            PrecioPorNoche = 1
        };
        var alta_Habitacion =_repoHabitacion.Alta(Habitacion);
        Habitacion.idHabitacion = alta_Habitacion;

        Assert.NotEqual<uint>(0, alta_Habitacion);
        Assert.NotNull(_repoHabitacion.Detalle(alta_Habitacion));
    }
    [Fact]
    public void InformarHabitacionPorIdHotel()
    {
        var habitaciones = _repoHabitacion.InformarHabitacionPorIdHotel(2);

        Assert.NotNull(habitaciones);
        Assert.Contains(habitaciones, habitacion => habitacion.PrecioPorNoche == (decimal)20000.00);
    }

    [Fact]
    public void InformarHabitacionPorIdTipo()
    {
        var habitaciones = _repoHabitacion.InformarHabitacionPorIdTipo(3);

        Assert.NotNull(habitaciones);
        Assert.Contains(habitaciones, habitacion => habitacion.PrecioPorNoche == (decimal)30000.00);
    }
    
    
    [Fact]
    public async Task InformarHabitacionPorIdAsync()
    {
        var detalle = await _repoHabitacionAsync.DetalleAsync(1);

        Assert.NotNull(detalle);
        Assert.Equal(detalle.PrecioPorNoche, 10000);

    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task InformarCiudadAsync(uint idHabitacion)
    {
        var habitaciones = await _repoHabitacionAsync.ListarAsync();

        Assert.NotEmpty(habitaciones);
        
        Assert.Contains(habitaciones, habitacion => habitacion.idHabitacion == idHabitacion);
    }
    [Fact]
     public async Task InsertarAsync()
    {
        var tipoHabitacion = await _repoTipoHabitacionAsync.DetalleAsync(1);
        var hotel = await _repoHotelAsync.DetalleAsync(2);
        Habitacion Habitacion = new Habitacion
        {
            tipoHabitacion = tipoHabitacion,
            hotel = hotel,
            PrecioPorNoche = 1
        };
        var alta_Habitacion = await _repoHabitacionAsync.AltaAsync(Habitacion);
        Habitacion.idHabitacion = alta_Habitacion;

        Assert.NotEqual<uint>(0, alta_Habitacion);
        Assert.NotNull(await _repoComentarioAsync.DetalleAsync(alta_Habitacion));
    }

        [Fact]
    public async Task InformarHabitacionPorIdHotelAsync()
    {
        var habitaciones = await _repoHabitacionAsync.InformarHabitacionPorIdHotelAsync(2);

        Assert.NotNull(habitaciones);
        Assert.Contains(habitaciones, habitacion => habitacion.PrecioPorNoche == (decimal)20000.00);
    }

    [Fact]
    public async Task InformarHabitacionPorIdTipoAsync()
    {
        var habitaciones = await  _repoHabitacionAsync.InformarHabitacionPorIdTipoAsync(3);

        Assert.NotNull(habitaciones);
        Assert.Contains(habitaciones, habitacion => habitacion.PrecioPorNoche == (decimal)30000.00);
    }
}
