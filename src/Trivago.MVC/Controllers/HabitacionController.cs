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
using MySqlConnector;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Trivago.MVC.Models;

namespace Trivago.MVC.Controllers;


public class HabitacionController : Controller
{
    private readonly IRepoHabitacionAsync _repoHabitacionAsync;
    private readonly IRepoHotelAsync _repoHotelAsync;
    private readonly IRepoTipoHabitacionAsync _repoTipoHabitacionAsync;
    private readonly IRepoComentarioAsync _repoComentarioAsync;
    private readonly ILogger<HabitacionController> _logger;

    public HabitacionController(ILogger<HabitacionController> logger, IRepoHabitacionAsync repoHabitacionAsync, IRepoHotelAsync repoHotelAsync, IRepoTipoHabitacionAsync repoTipoHabitacionAsync, IRepoComentarioAsync repoComentarioAsync)
    {
        _logger = logger;
        _repoComentarioAsync = repoComentarioAsync;
        _repoHabitacionAsync = repoHabitacionAsync;
        _repoHotelAsync = repoHotelAsync;
        _repoTipoHabitacionAsync = repoTipoHabitacionAsync;
    }


    public async Task<IActionResult> Index()
    {
        HabitacionGetViewModel habitacionGetViewModel = new()
        {
            Habitaciones = await _repoHabitacionAsync.ListarAsync()
        };
        HabitacionViewModel habitacionViewModel = new()
        {
            habitacionGetViewModel = habitacionGetViewModel
        };
        return View(habitacionViewModel);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> PostForm(uint? idHotel = 0, uint? idTipo = 0)
    {
        var hoteles = await _repoHotelAsync.ListarAsync();
        var tipoHabitaciones = await _repoTipoHabitacionAsync.ListarAsync(); 
        HabitacionViewModel habitacionViewModel = new()
        {

            habitacionPostViewModel = new()
            {
                IdHotelSeleccionado = idHotel,
                IdTipoHabitacionSeleccionado = idTipo,
                hoteles = new SelectList(items: hoteles.ToList(), dataValueField: nameof(Hotel.idHotel), dataTextField: nameof(Hotel.Nombre), selectedValue: idHotel),
                tipoHabitaciones = new SelectList(items: tipoHabitaciones.ToList(), dataValueField: nameof(TipoHabitacion.idTipo), dataTextField: nameof(TipoHabitacion.Nombre), selectedValue: idTipo)
            }
        };
        return View(habitacionViewModel);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> PostForm(HabitacionViewModel habitacionViewModel)
    {
        try
        {
            Habitacion habitacion = new Habitacion()
            {
                hotel = new() { idHotel = (uint)habitacionViewModel.habitacionPostViewModel.IdHotelSeleccionado },
                PrecioPorNoche = (decimal)habitacionViewModel.habitacionPostViewModel.PrecioPorNoche,
                tipoHabitacion = new() { idTipo = (uint)habitacionViewModel.habitacionPostViewModel.IdTipoHabitacionSeleccionado }
            };
            var id = await _repoHabitacionAsync.AltaAsync(habitacion);
            var habitaciones = await _repoHabitacionAsync.ListarAsync();
            habitacionViewModel.habitacionGetViewModel.Habitaciones = habitaciones;
            return View("Index", habitacionViewModel);
        }
        catch (MySqlException ex)
        {
            var hoteles = await _repoHotelAsync.ListarAsync();
            var tipoHabitaciones = await _repoTipoHabitacionAsync.ListarAsync();
            habitacionViewModel.habitacionPostViewModel.hoteles = new SelectList(items: hoteles.ToList(), dataValueField: nameof(Hotel.idHotel), dataTextField: nameof(Hotel.Nombre), selectedValue: habitacionViewModel.habitacionPostViewModel.IdHotelSeleccionado);
            habitacionViewModel.habitacionPostViewModel.tipoHabitaciones = new SelectList(items: tipoHabitaciones.ToList(), dataValueField: nameof(TipoHabitacion.idTipo), dataTextField: nameof(TipoHabitacion.Nombre), selectedValue: habitacionViewModel.habitacionPostViewModel.IdTipoHabitacionSeleccionado);
            ViewBag.Error = "Precio fuera del rango";
            return View(habitacionViewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(uint? id)
    {
        if (id == null)
        {
            return View(new Habitacion());
        }
        else
        {
            var habitacion = await _repoHabitacionAsync.DetalleAsync(id.Value);
            return View(habitacion);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Comentar(uint idHabitacion, sbyte Calificacion, string Texto)
    {
        var habitacion = await _repoHabitacionAsync.DetalleAsync(idHabitacion);
        
        if (habitacion == null)
            return NotFound();

        Comentario comentario = new()
        {
            Calificacion = Calificacion,
            Habitacion = idHabitacion,
            Fecha = DateTime.Now,
            comentario = Texto
        };
        Console.WriteLine(comentario.Calificacion);

        await _repoComentarioAsync.AltaAsync(comentario);

        return RedirectToAction("Detalle", new { id = idHabitacion });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}