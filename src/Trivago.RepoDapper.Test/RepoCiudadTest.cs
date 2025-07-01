using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper.Test;

public class RepoCiudadTest : TestBase
{
    private readonly IRepoCiudad _repoCiudad;
    private readonly IRepoHotel _repoHotel;
    private readonly IRepoPais _repoPais;

    private readonly IRepoCiudadAsync _repoCiudadAsync;
    private readonly IRepoHotelAsync _repoHotelAsync;
    private readonly IRepoPaisAsync _repoPaisAsync;
    public RepoCiudadTest() : base()
    {
        _repoCiudad = new RepoCiudad(Conexion);
        _repoHotel = new RepoHotel(Conexion);
        _repoPais = new RepoPais(Conexion);
        _repoCiudadAsync = new RepoCiudadAsync(Conexion);
        _repoHotelAsync = new RepoHotelAsync(Conexion);
        _repoPaisAsync = new RepoPaisAsync(Conexion);
    }

    [Fact]
    public void InformarCiudadPorId()
    {
        var detalle = _repoCiudad.Detalle(1);

        Assert.NotNull(detalle);
        Assert.Equal(detalle.Nombre, "Buenos Aires");


    }
    [Theory]
    [InlineData("Buenos Aires")]
    [InlineData("Mendoza")]
    [InlineData("Santiago del Estero")]
    public void InformarCiudad(string nombreCiudad)
    {
        var ciudades = _repoCiudad.Listar();
        
        Assert.NotEmpty(ciudades);
        Assert.Contains(ciudades, ciudad => ciudad.Nombre == nombreCiudad);
    }
    [Fact]
    public void Insertar()
    {
        var hoteles = _repoHotel.Listar();
        var francia = "Francia";
        var idFrancia = _repoPais.DetallePorNombre(francia).idPais;
        var ciudad = new Ciudad{ Hoteles = hoteles, idCiudad = 0, idPais = idFrancia, Nombre = "Paris"};        
        var idOUT = _repoCiudad.Alta(ciudad);

        Assert.NotEqual<uint>(0, ciudad.idCiudad);
        Assert.NotNull(_repoCiudad.Detalle(idOUT));
    }
    [Theory]
    [InlineData("Buenos Aires")]
    [InlineData("Mendoza")]
    [InlineData("Santiago del Estero")]
    public void informarciudadporpais(string nombreCiudad)
    {
        var ciudades = _repoCiudad.InformarCiudadPorIdPais(1);
        
        Assert.NotEmpty(ciudades);
        Assert.Contains(ciudades, ciudad => ciudad.Nombre == nombreCiudad);
    }

    
    [Fact]
    public async Task InformarCiudadPorIdAsync()
    {
        var detalle = await _repoCiudadAsync.DetalleAsync(1);

        Assert.NotNull(detalle);
        Assert.Equal(detalle.Nombre, "Buenos Aires");


    }
    [Theory]
    [InlineData("Buenos Aires")]
    [InlineData("Mendoza")]
    [InlineData("Santiago del Estero")]
    public async Task InformarCiudadAsync(string nombreCiudad)
    {
        var ciudades = await _repoCiudadAsync.ListarAsync();
        
        Assert.NotNull(ciudades);
        Assert.Contains(ciudades, ciudad => ciudad.Nombre == nombreCiudad);
    }
    [Fact]
    public async Task InsertarAsync()
    {
        var hoteles = await _repoHotelAsync.ListarAsync();
        var francia = "Francia";
        var idFrancia = await _repoPaisAsync.DetallePorNombreAsync(francia);
        var ciudad = new Ciudad{ Hoteles = hoteles, idCiudad = 0, idPais = idFrancia.idPais, Nombre = "Paris"};        
        var idOUT = await _repoCiudadAsync.AltaAsync(ciudad);

        Assert.NotEqual<uint>(0, ciudad.idCiudad);
        Assert.NotNull(await _repoCiudadAsync.DetalleAsync(idOUT));
    }
    [Theory]
    [InlineData("Buenos Aires")]
    [InlineData("Mendoza")]
    [InlineData("Santiago del Estero")]
    public async Task informarciudadporpaisAsync(string nombreCiudad)
    {
        var ciudades = await _repoCiudadAsync.InformarCiudadPorIdPaisAsync(1);
        
        Assert.NotEmpty(ciudades);
        Assert.Contains(ciudades, ciudad => ciudad.Nombre == nombreCiudad);
    }
}

