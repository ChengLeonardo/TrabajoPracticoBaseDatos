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

namespace Trivago.MVC.Controllers;

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
        var reservas = await _repoReservaAsync.ListarAsync();
        reservas = reservas.Where(r => r.idUsuario == Convert.ToUInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)).ToList();
        ReservaGetViewModel reservaGetViewModel = new()
        {
            Reservas = reservas
        };
        ReservaViewModel reservaViewModel = new()
        {
            reservaGetViewModel = reservaGetViewModel
        };
        return View(reservaViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> PostForm(uint? idHabitacion = 0)
    {
        var habitaciones = await _repoHabitacionAsync.ListarAsync();
        var metodoPagos = await _repoMetodoPagoAsync.ListarAsync();
        ReservaViewModel reservaViewModel = new()
        {

            reservaPostViewModel = new()
            {
                IdHabitacionSeleccionado = idHabitacion,
                habitaciones = new SelectList(items: habitaciones.ToList(), dataValueField: nameof(Habitacion.idHabitacion), dataTextField: "tipoHabitacion.Nombre", selectedValue: idHabitacion),
                metodosPago = new SelectList(items: metodoPagos.ToList(), dataValueField: nameof(MetodoPago.idMetodoPago), dataTextField: nameof(MetodoPago.TipoMedioPago))
            }
        };
        return View(reservaViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> PostForm(ReservaViewModel reservaViewModel)
    {
        var Precio = reservaViewModel.reservaPostViewModel.entrada.Subtract(reservaViewModel.reservaPostViewModel.salida);
        var habitacion = await _repoHabitacionAsync.DetalleAsync((uint)reservaViewModel.reservaPostViewModel.IdHabitacionSeleccionado);

        Reserva reserva = new Reserva()
        {
            habitacion = new() { idHabitacion = reservaViewModel.reservaPostViewModel.IdHabitacionSeleccionado.Value },
            metodoPago = new() { idMetodoPago = reservaViewModel.reservaPostViewModel.IdMetodoPagoSeleccionado.Value },
            idUsuario = Convert.ToUInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
            Salida = reservaViewModel.reservaPostViewModel.salida,
            Entrada = reservaViewModel.reservaPostViewModel.entrada,
            Telefono = reservaViewModel.reservaPostViewModel.Telefono.Value,
            Precio = Precio.Days * habitacion.PrecioPorNoche
        };
        var id = await _repoReservaAsync.AltaAsync(reserva);
        return RedirectToAction("Index");
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