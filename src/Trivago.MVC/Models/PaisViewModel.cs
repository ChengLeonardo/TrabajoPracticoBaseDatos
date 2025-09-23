using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.MVC.Models;

public class PaisViewModel
{
    public PaisGetViewModel? paisGetViewModel { get; set; } = new();
    public PaisPostViewModel? paisPostViewModel { get; set; } = new();

}

public class PaisPostViewModel
{
    public string nombre { get; set; }
}

public class PaisGetViewModel
{
    public List<Pais> Paises { get; set; } = new();

}