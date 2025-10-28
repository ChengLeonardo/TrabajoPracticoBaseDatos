using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Trivago.MVC.Controllers
{
    [Route("[controller]")]
    public class MetodoPagoController : Controller
    {
        private readonly ILogger<MetodoPagoController> _logger;

        public MetodoPagoController(ILogger<MetodoPagoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}