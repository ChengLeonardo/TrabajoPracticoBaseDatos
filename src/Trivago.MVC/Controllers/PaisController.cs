using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Trivago.MVC.Models;
using Trivago.RepoDapper;

namespace Trivago.MVC.Controllers
{
    [Authorize]
    public class PaisController : Controller
    {
        private readonly IRepoPaisAsync _repoPaisAsync;
        private readonly IRepoCiudadAsync _repoCiudadAsync;
        private readonly ILogger<PaisController> _logger;

        public PaisController(ILogger<PaisController> logger, IRepoPaisAsync repoPaisAsync, IRepoCiudadAsync repoCiudadAsync)
        {
            _logger = logger;
            _repoCiudadAsync = repoCiudadAsync;
            _repoPaisAsync = repoPaisAsync;
        }

        public async Task<IActionResult> Index()
        {
            PaisGetViewModel paisGetViewModel = new()
            {
                Paises = await _repoPaisAsync.ListarAsync()
            };
            PaisViewModel paisViewModel = new()
            {
                paisGetViewModel = paisGetViewModel
            };
            return View(paisViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PostForm()
        {
            var paises = await _repoPaisAsync.ListarAsync();
            PaisViewModel paisViewModel = new()
            {
                paisPostViewModel = new()
            };
            return View(paisViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostForm(PaisViewModel paisViewModel)
        {
            Pais pais = new Pais()
            {
                Nombre = paisViewModel.paisPostViewModel.nombre
            };
            var id = await _repoPaisAsync.AltaAsync(pais);
            var paises = await _repoPaisAsync.ListarAsync();
            paisViewModel.paisGetViewModel.Paises = paises;
            return View("Index", paisViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(uint? id)
        {
            if (id == null)
            {
                return View(new Pais());
            }
            else
            {
                var pais = await _repoPaisAsync.DetalleAsync(id.Value);
                return View(pais);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}