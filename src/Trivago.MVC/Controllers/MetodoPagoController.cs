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
    [Authorize(Roles = "Admin")]

    public class MetodoPagoController : Controller
    {
        private readonly IRepoMetodoPagoAsync _repoMetodoPagoAsync;
        private readonly IRepoCiudadAsync _repoCiudadAsync;
        private readonly ILogger<MetodoPagoController> _logger;

        public MetodoPagoController(ILogger<MetodoPagoController> logger, IRepoMetodoPagoAsync repoMetodoPagoAsync, IRepoCiudadAsync repoCiudadAsync)
        {
            _logger = logger;
            _repoCiudadAsync = repoCiudadAsync;
            _repoMetodoPagoAsync = repoMetodoPagoAsync;
        }

        public async Task<IActionResult> Index()
        {
            MetodoPagoGetViewModel metodoPagoGetViewModel = new()
            {
                metodoPagos = await _repoMetodoPagoAsync.ListarAsync()
            };
            MetodoPagoViewModel metodoPagoViewModel = new()
            {
                metodoPagoGetViewModel = metodoPagoGetViewModel
            };
            return View(metodoPagoViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> PostForm()
        {
            MetodoPagoViewModel metodoPagoViewModel = new()
            {
                metodoPagoPostViewModel = new()
            };
            return View(metodoPagoViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostForm(MetodoPagoViewModel metodoPagoViewModel)
        {
            MetodoPago metodoPago = new MetodoPago()
            {
                TipoMedioPago = metodoPagoViewModel.metodoPagoPostViewModel.nombre
            };
            var id = await _repoMetodoPagoAsync.AltaAsync(metodoPago);
            var metodoPagoes = await _repoMetodoPagoAsync.ListarAsync();
            metodoPagoViewModel.metodoPagoGetViewModel.metodoPagos = metodoPagoes;
            return View("Index", metodoPagoViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(uint? id)
        {
            if (id == null)
            {
                return View(new MetodoPago());
            }
            else
            {
                var metodoPago = await _repoMetodoPagoAsync.DetalleAsync(id.Value);
                return View(metodoPago);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}