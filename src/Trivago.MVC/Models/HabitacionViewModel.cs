using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.MVC.Models;

public class HabitacionViewModel
{
    public HabitacionGetViewModel? habitacionGetViewModel { get; set; } = new();
    public HabitacionPostViewModel? habitacionPostViewModel { get; set; } = new();

}

public class HabitacionPostViewModel
{
    public SelectList tipoHabitaciones { get; set; }
    public SelectList hoteles { get; set;  }
    public decimal PrecioPorNoche { get; set; }

    public uint? IdHotelSeleccionado { get; set; }
    public uint? IdTipoHabitacionSeleccionado { get; set; }

}

public class HabitacionGetViewModel
{
    public List<Habitacion> Habitaciones { get; set; } = new();

}