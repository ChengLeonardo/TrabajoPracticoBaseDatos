using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Trivago.MVC.Models;

namespace Trivago.MVC.Controllers
{
    [Authorize]
    public class ReservaController : Controller
    {
        private readonly IRepoReservaAsync _repoReservaAsync;
        private readonly IRepoHabitacionAsync _repoHabitacionAsync;
        private readonly IRepoMetodoPagoAsync _repoMetodoPagoAsync;
        private readonly ILogger<ReservaController> _logger;

        public ReservaController(ILogger<ReservaController> logger, IRepoReservaAsync repoReservaAsync, IRepoHabitacionAsync repoHabitacionAsync, IRepoMetodoPagoAsync repoMetodoPagoAsync)
        {
            _logger = logger;
            _repoReservaAsync = repoReservaAsync;
            _repoHabitacionAsync = repoHabitacionAsync;
            _repoMetodoPagoAsync = repoMetodoPagoAsync;
        }

        public async Task<IActionResult> Index()
        {
            ReservaGetViewModel reservaGetViewModel = new()
            {
                Reservas = await _repoReservaAsync.ListarAsync()
            };
            ReservaViewModel reservaViewModel = new()
            {
                reservaGetViewModel = reservaGetViewModel
            };
            return View(reservaViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PostForm()
        {
            var habitaciones = await _repoHabitacionAsync.ListarAsync();
            var metodoPagos = await _repoMetodoPagoAsync.ListarAsync();
            ReservaViewModel reservaViewModel = new()
            {

                reservaPostViewModel = new()
                {
                    habitaciones = new SelectList(items: habitaciones.ToList(), dataValueField: nameof(Habitacion.idHabitacion), dataTextField: nameof(Habitacion.tipoHabitacion)),
                    metodosPago = new SelectList(items: metodoPagos.ToList(), dataValueField: nameof(MetodoPago.idMetodoPago), dataTextField: nameof(MetodoPago.TipoMedioPago))
                }
            };
            return View(reservaViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostForm(ReservaViewModel reservaViewModel)
        {
            Reserva reserva = new Reserva()
            {
                habitacion = new() { idHabitacion = reservaViewModel.reservaPostViewModel.IdHabitacionSeleccionado.Value },
                metodoPago = new() { idMetodoPago = reservaViewModel.reservaPostViewModel.IdMetodoPagoSeleccionado.Value },
                idUsuario = Convert.ToUInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Salida = reservaViewModel.reservaPostViewModel.salida,
                Entrada = reservaViewModel.reservaPostViewModel.entrada,
                Telefono = reservaViewModel.reservaPostViewModel.Telefono.Value
            };
            var id = await _repoReservaAsync.AltaAsync(reserva);
            var reservaes = await _repoReservaAsync.ListarAsync();
            reservaViewModel.reservaGetViewModel.Reservas = reservaes;
            return View("Index", reservaViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(uint? id)
        {
            if (id == null)
            {
                return View(new Reserva());
            }
            else
            {
                var reserva = await _repoReservaAsync.DetalleAsync(id.Value);
                return View(reserva);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}