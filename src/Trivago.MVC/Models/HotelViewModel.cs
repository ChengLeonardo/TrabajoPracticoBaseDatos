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
        internal HotelGetViewModel? hotelGetViewModel { get; set; } = new();
        internal HotelPostViewModel? hotelPostViewModel { get; set; } = new();
    }

    public class HotelGetViewModel
    {
        public List<Hotel> hoteles { get; set; } = new();
    }
    public class HotelPostViewModel
    {
        internal string nombre { get; set; }

        internal SelectList ciudades { get; set; } 

        public uint? IdCiudadSeleccionado { get; set; }
        public string URL { get;  set; }
        public string Telefono { get;  set; }
        public string Direccion { get;  set; }
    }
}