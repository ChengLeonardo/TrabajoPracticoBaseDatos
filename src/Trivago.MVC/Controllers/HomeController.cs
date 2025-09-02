using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trivago.MVC.Models;
using Trivago.RepoDapper;
using Trivago.Core;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using System.Threading.Tasks;
namespace Trivago.MVC.Controllers;

public class HomeController : Controller
{
    private readonly IRepoPaisAsync _repoPais;
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger, IRepoPaisAsync repoPais)
    {
        _logger = logger;
        _repoPais = repoPais;
    }

    public async Task<IActionResult> Index()
    {
        var paises = await _repoPais.ListarAsync();
        return View(paises);
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
