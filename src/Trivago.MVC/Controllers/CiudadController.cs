using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Trivago.MVC.Models;
using Trivago.RepoDapper;

namespace Trivago.MVC.Controllers
{
    public class CiudadController : Controller
    {
        private readonly IRepoCiudadAsync _repoCiudadAsync;
        private readonly IRepoPaisAsync _repoPaisAsync;
        private readonly ILogger<CiudadController> _logger;

        public CiudadController(ILogger<CiudadController> logger, IRepoCiudadAsync repoCiudadAsync, IRepoPaisAsync  repoPaisAsync)
        {
            _logger = logger;
            _repoPaisAsync = repoPaisAsync;
            _repoCiudadAsync = repoCiudadAsync;
        }

        public async Task<IActionResult> Index()
        {
            CiudadGetViewModel ciudadGetViewModel = new()
            {
                ciudades = await _repoCiudadAsync.ListarAsync()
            };
            CiudadViewModel ciudadViewModel = new()
            {
                ciudadGetViewModel = ciudadGetViewModel
            };
            return View(ciudadViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PostForm()
        {
            var paises = await _repoPaisAsync.ListarAsync();
            CiudadViewModel ciudadViewModel = new()
            {
                ciudadPostViewModel = new()
            };
            ciudadViewModel.ciudadPostViewModel.paises = new SelectList(items: paises.ToList(), dataValueField: nameof(Pais.idPais), dataTextField: nameof(Pais.Nombre));
            return View(ciudadViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostForm(CiudadViewModel ciudadViewModel)
        {
            Ciudad ciudad = new Ciudad()
            {
                idPais = ciudadViewModel.ciudadPostViewModel.IdPaisSeleccionado.Value,
                nombre = ciudadViewModel.ciudadPostViewModel.nombre
            };
            var id = await _repoCiudadAsync.AltaAsync(ciudad);
                        var ciudades = await _repoCiudadAsync.ListarAsync();
            ciudadViewModel.ciudadGetViewModel.ciudades = ciudades;
            return View("Index", ciudadViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}