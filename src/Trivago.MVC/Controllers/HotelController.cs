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
    public class HotelController : Controller
    {
        private readonly IRepoHotelAsync _repoHotelAsync;
        private readonly IRepoPaisAsync _repoPaisAsync;
        private readonly IRepoCiudadAsync _repoCiudadAsync;
        private readonly ILogger<HotelController> _logger;

        public HotelController(ILogger<HotelController> logger, IRepoHotelAsync repoHotelAsync, IRepoPaisAsync repoPaisAsync, IRepoCiudadAsync repoCiudadAsync)
        {
            _logger = logger;
            _repoPaisAsync = repoPaisAsync;
            _repoCiudadAsync = repoCiudadAsync;
            _repoHotelAsync = repoHotelAsync;
        }

        public async Task<IActionResult> Index()
        {
            HotelGetViewModel hotelGetViewModel = new()
            {
                hoteles = await _repoHotelAsync.ListarAsync()
            };
            HotelViewModel hotelViewModel = new()
            {
                hotelGetViewModel = hotelGetViewModel
            };
            return View(hotelViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PostForm()
        {
            var ciudades = await _repoCiudadAsync.ListarAsync();
            HotelViewModel hotelViewModel = new()
            {
                hotelPostViewModel = new()
            };
            hotelViewModel.hotelPostViewModel.ciudades = new SelectList(items: ciudades.ToList(), dataValueField: nameof(Ciudad.idCiudad), dataTextField: nameof(Ciudad.nombre));
            return View(hotelViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostForm(HotelViewModel hotelViewModel)
        {
            if (hotelViewModel.hotelPostViewModel.IdCiudadSeleccionado == null)
            {
                Console.WriteLine("es nullo");
            }
            Hotel hotel = new Hotel()
            {
                idCiudad = hotelViewModel.hotelPostViewModel.IdCiudadSeleccionado.Value,
                Nombre = hotelViewModel.hotelPostViewModel.nombre,
                URL = hotelViewModel.hotelPostViewModel.URL,
                Telefono = hotelViewModel.hotelPostViewModel.Telefono,
                Direccion = hotelViewModel.hotelPostViewModel.Direccion
            };
            var id = await _repoHotelAsync.AltaAsync(hotel);
            var hoteles = await _repoHotelAsync.ListarAsync();
            hotelViewModel.hotelGetViewModel.hoteles = hoteles;
            return View("Index", hotelViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(uint? id)
        {
            if (id == null)
            {
                return View(new Hotel());
            }
            else
            {
                var hotel = await _repoHotelAsync.DetalleAsync(id.Value);
                return View(hotel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}