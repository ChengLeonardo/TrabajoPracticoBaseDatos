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
public class HabitacionController : Controller
{
    private readonly IRepoHabitacionAsync _repoHabitacionAsync;
    private readonly IRepoHotelAsync _repoHotelAsync;
    private readonly IRepoTipoHabitacionAsync _repoTipoHabitacionAsync;
    private readonly ILogger<HabitacionController> _logger;

    public HabitacionController(ILogger<HabitacionController> logger, IRepoHabitacionAsync repoHabitacionAsync, IRepoHotelAsync repoHotelAsync, IRepoTipoHabitacionAsync repoTipoHabitacionAsync)
    {
        _logger = logger;
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
    
    [HttpGet]
    public async Task<IActionResult> PostForm()
    {
        var hoteles = await _repoHotelAsync.ListarAsync();
        var tipoHabitaciones = await _repoTipoHabitacionAsync.ListarAsync(); 
        HabitacionViewModel habitacionViewModel = new()
        {

            habitacionPostViewModel = new()
            {
                hoteles = new SelectList(items: hoteles.ToList(), dataValueField: nameof(Hotel.idHotel), dataTextField: nameof(Hotel.Nombre)),
                tipoHabitaciones = new SelectList(items: tipoHabitaciones.ToList(), dataValueField: nameof(TipoHabitacion.idTipo), dataTextField: nameof(TipoHabitacion.Nombre))
            }
        };
        return View(habitacionViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> PostForm(HabitacionViewModel habitacionViewModel)
    {
        
        Habitacion habitacion = new Habitacion()
        {
            hotel = new() { idHotel = (uint)habitacionViewModel.habitacionPostViewModel.IdHotelSeleccionado },
            PrecioPorNoche = habitacionViewModel.habitacionPostViewModel.PrecioPorNoche,
            tipoHabitacion = new() { idTipo = (uint)habitacionViewModel.habitacionPostViewModel.IdHotelSeleccionado}
        };
        var id = await _repoHabitacionAsync.AltaAsync(habitacion);
        var habitaciones = await _repoHabitacionAsync.ListarAsync();
        habitacionViewModel.habitacionGetViewModel.Habitaciones = habitaciones;
        return View("Index", habitacionViewModel);
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}