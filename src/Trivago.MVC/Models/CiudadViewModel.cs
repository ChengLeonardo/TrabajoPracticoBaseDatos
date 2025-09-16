using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.MVC.Models;

public class CiudadViewModel
{
    public CiudadGetViewModel? ciudadGetViewModel { get; set; } = new();
    public CiudadPostViewModel? ciudadPostViewModel { get; set; } = new();

}

public class CiudadPostViewModel
{
    public string nombre { get; set; }
    public SelectList paises { get; set; }
    public uint? IdPaisSeleccionado { get; set; }
}

public class CiudadGetViewModel
{
    public List<Ciudad> ciudades { get; set; } = new();

}