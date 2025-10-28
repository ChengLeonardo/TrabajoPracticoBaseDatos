using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trivago.Core.Ubicacion;

namespace Trivago.MVC.Models
{

    public class ReservaViewModel
    {
        public ReservaGetViewModel? reservaGetViewModel { get; set; } = new();
        public ReservaPostViewModel? reservaPostViewModel { get; set; } = new();

    }

    public class ReservaPostViewModel
    {
        public SelectList habitaciones { get; set; }

        public uint? IdHabitacionSeleccionado { get; set; }
        public SelectList metodosPago { get; set; }

        public uint? IdMetodoPagoSeleccionado { get; set; }
        public DateTime entrada { get; set; }
        public DateTime salida { get; set; }
        public uint? Telefono { get; set; }
    }

    public class ReservaGetViewModel
    {
        public List<Reserva> Reservas { get; set; } = new();

    }
}