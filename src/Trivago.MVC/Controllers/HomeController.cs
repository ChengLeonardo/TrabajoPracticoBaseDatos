using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trivago.MVC.Models;
using Trivago.RepoDapper;
using Trivago.Core;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;

namespace Trivago.MVC.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly IRepoPaisAsync _repoPaisAsync;

    private readonly IRepoCiudadAsync _repoCiudadAsync;
    private readonly IRepoHotelAsync _repoHotelAsync;
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger, IRepoPaisAsync repoPais, IRepoCiudadAsync repoCiudadAsync, IRepoHotelAsync repoHotelAsync)
    {
        _logger = logger;
        _repoPaisAsync = repoPais;
        _repoCiudadAsync = repoCiudadAsync;
        _repoHotelAsync = repoHotelAsync;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var paises = await _repoPaisAsync.ListarAsync();
        var ciudades = await _repoCiudadAsync.ListarAsync();
        var hoteles = await _repoHotelAsync.ListarAsync();
        HomeViewModel homeViewModel = new()
        {
            homePostViewModel = new()
        };
        List<object> Lugares =
        [
            .. paises.Select(x => new { id = x.idPais + x.Nombre + typeof(Pais), nombre = x.Nombre, tipo = typeof(Pais) }).ToList(),
            .. ciudades.Select(x => new { id = x.idCiudad + x.nombre +  typeof(Ciudad), nombre = x.nombre, tipo = typeof(Ciudad) }).ToList(),
            .. hoteles.Select(x => new { id = x.idHotel + x.Nombre + typeof(Hotel), nombre = x.Nombre, tipo = typeof(Hotel) }).ToList(),
        ];

        homeViewModel.homePostViewModel.LugaresDato = Lugares;
        homeViewModel.homePostViewModel.Lugares = new SelectList(items: Lugares.ToList(), dataValueField: "id", dataTextField: "nombre");
        return View(homeViewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Index(HomeViewModel homeViewModel)
    {
        var paises = await _repoPaisAsync.ListarAsync();
        var ciudades = await _repoCiudadAsync.ListarAsync();
        var hoteles = await _repoHotelAsync.ListarAsync();
        List<object> Lugares =
        [
            .. paises.Select(x => new { id = x.idPais + x.Nombre + typeof(Pais), nombre = x.Nombre, tipo = "Pais" }).ToList(),
            .. ciudades.Select(x => new { id = x.idCiudad + x.nombre +  typeof(Ciudad), nombre = x.nombre, tipo = "Ciudad" }).ToList(),
            .. hoteles.Select(x => new { id = x.idHotel + x.Nombre + typeof(Hotel), nombre = x.Nombre, tipo = "Hotel" }).ToList(),
        ];

        var seleccionado = Lugares
        .Cast<dynamic>()
        .FirstOrDefault(x => x.id == homeViewModel.homePostViewModel.id);
        string idStr = seleccionado.id.ToString();

        // tomar solo los caracteres iniciales que sean dígitos
        string numeros = new string(idStr.TakeWhile(char.IsDigit).ToArray());

        // convertir a uint
        uint? id = uint.Parse(numeros);
        Console.WriteLine(id);
        Console.WriteLine(id.GetType());
        switch ((string)seleccionado.tipo)
        {
            case "Pais":
                return RedirectToAction("Detalle", "Pais", new { id });
            case "Ciudad":
                return RedirectToAction("Detalle", "Ciudad",  new { id });
            case "Hotel":
                return RedirectToAction("Detalle", "Hotel",  new { id });
        }
        return View(homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
