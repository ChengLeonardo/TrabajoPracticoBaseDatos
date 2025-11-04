using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Trivago.MVC.Models;

namespace Trivago.MVC.Controllers;

public class UsuarioController : Controller
{
    public IRepoUsuarioAsync _repoUsuarioAsync;
    public UsuarioController(IRepoUsuarioAsync repoUsuarioAsync)
    {
        _repoUsuarioAsync = repoUsuarioAsync;
    }

    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home"); 
        }
        return View();
    }
    public IActionResult Registrar()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home"); 
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Registrar(UsuarioViewModel usuarioViewModel)
    {
        Usuario usuario = new Usuario()
        {
            Mail = usuarioViewModel.email,
            Nombre = usuarioViewModel.nombre,
            Contrasena = usuarioViewModel.pass,
            Apellido = usuarioViewModel.apellido
        };
        await _repoUsuarioAsync.AltaAsync(usuario);
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.idUsuario.ToString(), ClaimValueTypes.String),
            new Claim(ClaimTypes.Email, usuario.Mail, ClaimValueTypes.String),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Login(UsuarioViewModel usuarioViewModel)
    {
        var usuario = await _repoUsuarioAsync.UsuarioPorPassAsync(usuarioViewModel.email, usuarioViewModel.pass);
        if (usuario == null)
        {
            return View(usuarioViewModel);
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.idUsuario.ToString(), ClaimValueTypes.String),
            new Claim(ClaimTypes.Email, usuario.Mail, ClaimValueTypes.String),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("Login");
        }
            return RedirectToAction("Index", "Home"); 

    }

}