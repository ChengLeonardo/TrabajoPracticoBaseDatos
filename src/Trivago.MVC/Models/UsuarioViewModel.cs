using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trivago.MVC.Models;

public class UsuarioViewModel
{
    public string? apellido { get; set; }
    public string? pass { get; set; }
    public string? email { get; set; }
    public string? nombre { get; set; }
}