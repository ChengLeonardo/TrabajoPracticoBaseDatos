using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoReservaTest : TestBase
{
    private readonly IRepoReserva _repoReserva;
    private readonly IRepoHabitacion _repoHabitacion;
    private readonly IRepoMetodoPago _repoMetodoPago;

    private readonly IRepoReservaAsync _repoReservaAsync;
    private readonly IRepoHabitacionAsync _repoHabitacionAsync;
    private readonly IRepoMetodoPagoAsync _repoMetodoPagoAsync;
    public RepoReservaTest() : base()
    {
        _repoReserva = new RepoReserva(Conexion);
        _repoHabitacion = new RepoHabitacion(Conexion);
        _repoMetodoPago = new RepoMetodoPago(Conexion);
        _repoReservaAsync = new RepoReservaAsync(Conexion);
        _repoHabitacionAsync = new RepoHabitacionAsync(Conexion);
        _repoMetodoPagoAsync = new RepoMetodoPagoAsync(Conexion);
    }

    [Fact]
    public void InformarReservaPorId()
    {
        var detalle = _repoReserva.Detalle(1);

        Assert.NotNull(detalle);
        Assert.Equal("11111111", detalle.Telefono.ToString());
    }
    [Theory]
    [InlineData(11111111)]
    [InlineData(11111112)]
    [InlineData(11111113)]
    public void InformarReservas(uint numTelefono)
    {
        var reservas = _repoReserva.Listar();
        
        Assert.Contains(reservas, reserva => reserva.Telefono == numTelefono);
    }
    [Fact]
    public void Insertar()
    {
        var habitacion = _repoHabitacion.Detalle(1)!;
        var efectivo = _repoMetodoPago.Detalle(1)!;
        var reserva = new Reserva 
            {
                Entrada = new DateTime(2025, 02, 1),
                habitacion = habitacion,
                idReserva = 0,
                idUsuario = 1,
                metodoPago = efectivo,
                Salida = new DateTime(2025, 04, 2),
                Precio = 0,
                Telefono = 123456789
            };
        
        var idOUT = _repoReserva.Alta(reserva);
        
        Assert.NotEqual<uint>(0, reserva.idReserva);
        

    }

    [Fact]
    public void InformarReservasPorIdHabitacion()
    {
        var reservas = _repoReserva.InformarReservasPorIdHabitacion(2);
        
        Assert.Contains(reservas, reserva => reserva.Precio == 11111112);
    }

    [Fact]
    public void InformarReservasPorIdMetodoPago()
    {
        var reservas = _repoReserva.InformarReservasPorIdMetodoPago(3);
        
        Assert.Contains(reservas, reserva => reserva.Precio == 11111113);
    }

    
    [Fact]
    public async Task InformarReservaPorIdAsync()
    {
        var detalle = await _repoReservaAsync.DetalleAsync(1);

        Assert.NotNull(detalle);
        Assert.Equal("11111111", detalle.Telefono.ToString());
    }
    [Theory]
    [InlineData(11111111)]
    [InlineData(11111112)]
    [InlineData(11111113)]
    public async Task InformarReservasAsync(uint numTelefono)
    {
        var reservas = await _repoReservaAsync.ListarAsync();
        
        Assert.Contains(reservas, reserva => reserva.Telefono == numTelefono);
    }
    [Fact]
    public async Task InsertarAsync()
    {
        var habitacion = await _repoHabitacionAsync.DetalleAsync(1)!;
        var efectivo = await _repoMetodoPagoAsync.DetalleAsync(1)!;
        var reserva = new Reserva 
            {
                Entrada = new DateTime(2025, 02, 1),
                habitacion = habitacion,
                idReserva = 0,
                idUsuario = 1,
                metodoPago = efectivo,
                Salida = new DateTime(2025, 04, 2),
                Precio = 0,
                Telefono = 123456789
            };
        
        var idOUT = await _repoReservaAsync.AltaAsync(reserva);
        
        Assert.NotEqual<uint>(0, reserva.idReserva);
    }
}
