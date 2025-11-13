using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Trivago.MVC.Models;

namespace Trivago.MVC.Controllers;

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

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var reservas = await _repoReservaAsync.ListarAsync();
        if (User.IsInRole("Usuario"))
        {
            reservas = reservas.Where(r => r.idUsuario == Convert.ToUInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)).ToList();
        }
        else if (User.IsInRole("Hotel"))
        {
            reservas = reservas.Where(r => r.habitacion.hotel.Email == User.FindFirst(ClaimTypes.Email).Value).ToList();
        }
        Console.WriteLine("asdf");
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
    [Authorize]
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
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostForm(ReservaViewModel reservaViewModel)
    {
        var Precio = reservaViewModel.reservaPostViewModel.salida.Subtract(reservaViewModel.reservaPostViewModel.entrada);
        try
        {
            if (Precio.Days < 0)
            {
                throw new Exception("Fecha fin no puede ser anterior que el inicio");
            }
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
        }
        catch (MySqlException ex)
        {
            var habitaciones = await _repoHabitacionAsync.ListarAsync();
            var metodoPagos = await _repoMetodoPagoAsync.ListarAsync();
            reservaViewModel.reservaPostViewModel.habitaciones = new SelectList(items: habitaciones.ToList(), dataValueField: nameof(Habitacion.idHabitacion), dataTextField: "tipoHabitacion.Nombre", selectedValue: reservaViewModel.reservaPostViewModel.IdHabitacionSeleccionado);
            reservaViewModel.reservaPostViewModel.metodosPago = new SelectList(items: metodoPagos.ToList(), dataValueField: nameof(MetodoPago.idMetodoPago), dataTextField: nameof(MetodoPago.TipoMedioPago), selectedValue: reservaViewModel.reservaPostViewModel.IdMetodoPagoSeleccionado);
            if (ex.ToString().Contains("Out"))
            {
                ViewBag.Error = "Datos ingresados fuera del rango";
            }
            else
            {
                ViewBag.Error = ex.Message;
            }
            return View(reservaViewModel);
        }
        catch (Exception ex)
        {
            if (ex != null)
            {
                ViewBag.Error = ex.Message;
            }
            else
            {
                ViewBag.Error = "Datos ingresados incorrectos";

            }
            var habitaciones = await _repoHabitacionAsync.ListarAsync();
            var metodoPagos = await _repoMetodoPagoAsync.ListarAsync();
            reservaViewModel.reservaPostViewModel.habitaciones = new SelectList(items: habitaciones.ToList(), dataValueField: nameof(Habitacion.idHabitacion), dataTextField: "tipoHabitacion.Nombre", selectedValue: reservaViewModel.reservaPostViewModel.IdHabitacionSeleccionado);
            reservaViewModel.reservaPostViewModel.metodosPago = new SelectList(items: metodoPagos.ToList(), dataValueField: nameof(MetodoPago.idMetodoPago), dataTextField: nameof(MetodoPago.TipoMedioPago), selectedValue: reservaViewModel.reservaPostViewModel.IdMetodoPagoSeleccionado);
            return View(reservaViewModel);
        }
        return RedirectToAction("Index");
    }
    
    [Authorize]
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
            if (reserva.idUsuario != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            return View(reserva);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}