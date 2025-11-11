using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Trivago.Core.Persistencia;

namespace Trivago.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolController : Controller
    {
        private readonly ILogger<RolController> _logger;
        private readonly IRepoRolAsync _repoRolAsync;

        public RolController(ILogger<RolController> logger, IRepoRolAsync repoRolAsync)
        {
            _logger = logger;
            _repoRolAsync = repoRolAsync;
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