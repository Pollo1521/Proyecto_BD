using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Proyecto_BD.Models;
using Proyecto_BD.Utilities;

namespace Proyecto_BD.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ContextoBaseDatos _context;

    public HomeController(ILogger<HomeController> logger, ContextoBaseDatos context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Login

    public IActionResult Login()
    {
        ViewBag.Error = String.IsNullOrEmpty(HttpContext.Request.Query["error"]) ? false : true;
        ViewBag.Completo = String.IsNullOrEmpty(HttpContext.Request.Query["completo"]) ? false : true;
        return View();
    }

    [HttpPost]
    public IActionResult Login(string Correo, string Password)
    {
        var usuario = _context.Usuario.FirstOrDefault(u => u.Correo == Correo);

        if (usuario == null)
        {
            return RedirectToAction("Login", new { error = true });
        }

        if (usuario.Password != Password)
        // ASI DEBE QUEDAR
        // Utilerias.Encriptar.EncriptarPassword(Password))
        {
            return RedirectToAction("Login", new { error = true });
        }

        var autorizacion = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.ID_Tipo_Usuario.ToString()),
                new Claim("ID_Usuario", usuario.ID_Usuario.ToString()),
                new Claim("Rol", usuario.ID_Tipo_Usuario.ToString()),
                new Claim("Autenticado", "SI"),
            };

        var identidad = new ClaimsIdentity(autorizacion, "AutorizacionesClaim");
        var principal = new ClaimsPrincipal(new[] { identidad });

        HttpContext.SignInAsync(principal);

        switch (usuario.ID_Tipo_Usuario)
        {
            //AJUSTAAAAAAAAAAAAAAAAAAAAAAAAAAR

            //Admin
            //case 1:
            //    var admin = _context.Gestiones.FirstOrDefault(x => x.Usuario_ID == usuario.Usuario_ID);

            //    if (admin == null)
            //    {
            //        return RedirectToAction(controllerName: "Gestiones", actionName: "Registro");
            //    }

            //    return RedirectToAction(controllerName: "Usuarios", actionName: "DatosUsuario");

            //// Gestion
            //case 2:
            //    var gestion = _context.Gestiones.FirstOrDefault(x => x.Usuario_ID == usuario.Usuario_ID);

            //    if (gestion == null)
            //    {
            //        return RedirectToAction(controllerName: "Gestiones", actionName: "Registro");
            //    }

            //    return RedirectToAction(controllerName: "Usuarios", actionName: "DatosUsuario");

            ////Alumno
            //case 3:
            //    var alumno = _context.Alumnos.FirstOrDefault(a => a.Usuario_ID == usuario.Usuario_ID);

            //    if (alumno == null)
            //    {
            //        return RedirectToAction(controllerName: "Alumnos", actionName: "Registro");
            //    }

            //    return RedirectToAction(controllerName: "Usuarios", actionName: "DatosUsuario");

            default:
                
                if (usuario == null)
                {
                    return RedirectToAction(controllerName: "Gestiones", actionName: "Registro");
                }

                return RedirectToAction(controllerName: "Home", actionName: "Index");
        }
    }   
}
