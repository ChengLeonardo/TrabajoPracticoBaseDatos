using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trivago.Core.Ubicacion;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Trivago.MVC.Models
{
    public class HomeViewModel
    {
        public HomePostViewModel? homePostViewModel { get; set; }

    }

    public class HomePostViewModel
    {
        public List<object> LugaresDato { get; set; } = new();
        public SelectList Lugares { get; set; }
        public string? id { get; set; } = null;
    }
}