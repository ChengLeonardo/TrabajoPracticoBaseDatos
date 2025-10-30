using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trivago.Core.Ubicacion;

namespace Trivago.MVC.Models
{
    public class MetodoPagoViewModel
    {
        public MetodoPagoGetViewModel? metodoPagoGetViewModel { get; set; } = new();
        public MetodoPagoPostViewModel? metodoPagoPostViewModel { get; set; } = new();
    }

    public class MetodoPagoGetViewModel
    {
        public List<MetodoPago> metodoPagos { get; set; } = new();
    }
    public class MetodoPagoPostViewModel
    {
        public string nombre { get; set; }
    }
}