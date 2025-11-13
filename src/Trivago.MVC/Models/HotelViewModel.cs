using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trivago.Core.Ubicacion;

namespace Trivago.MVC.Models
{
    public class HotelViewModel
    {
        public HotelGetViewModel? hotelGetViewModel { get; set; } = new();
        public HotelPostViewModel? hotelPostViewModel { get; set; } = new();
    }

    public class HotelGetViewModel
    {
        public List<Hotel> hoteles { get; set; } = new();
    }
    public class HotelPostViewModel
    {
        public string nombre { get; set; }

        public SelectList ciudades { get; set; }

        public uint? IdCiudadSeleccionado { get; set; }
        public string URL { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}