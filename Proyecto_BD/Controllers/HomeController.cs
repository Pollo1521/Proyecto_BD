using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Proyecto_BD.Models;
using Proyecto_BD.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Proyecto_BD.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ContextoBaseDatos _context;
    private readonly CorreoElectronico _correoElectronico;

    public HomeController(ILogger<HomeController> logger, ContextoBaseDatos context, CorreoElectronico correoElectronico)
    {
        _logger = logger;
        _context = context;
        _correoElectronico =correoElectronico;
    }

    [Authorize(Roles = "1")]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "1")]
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

        if (usuario.Password != Encriptar.EncriptarPassword(Password))
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
            //Admin
            case 1:
                var admin = _context.Usuario.FirstOrDefault(x => x.ID_Usuario == usuario.ID_Usuario);

                return RedirectToAction(controllerName: "Home", actionName: "Index");

            // Paciente
            case 2:
                var paciente = _context.Paciente.FirstOrDefault(x => x.ID_Usuario == usuario.ID_Usuario);

                if (paciente == null)
                {
                    return RedirectToAction(controllerName: "Pacientes", actionName: "Create");
                }

                return RedirectToAction(controllerName: "Pacientes", actionName: "Index");

            //Doctor
            case 3:
                var medico = _context.Medico.FirstOrDefault(a => a.ID_Usuario == usuario.ID_Usuario);

                return RedirectToAction(controllerName: "Medicos", actionName: "Index");

            //Recepcionista
            case 4:
                var recepcionista = _context.Recepcionista.FirstOrDefault(a => a.ID_Usuario == usuario.ID_Usuario);

                return RedirectToAction("Login", "Recepcionistas");

            default:
                return NotFound();
        }
    }

    //Registro de Pacientes
    public IActionResult RegistroPaciente()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistroPaciente([Bind("ID_Usuario,Nombre,Apellido_Paterno,Apellido_Materno,Correo,CURP,Fecha_Nacimiento,Password")] Usuario usuario, string PasswordRepetir)
    {

        if(PasswordRepetir != usuario.Password)
        {
            ModelState.AddModelError("Password", "Las contraseñas no coinciden");
            return View(usuario);
        }

        //Validando si el correo ya existe
        var correoExistente = _context.Usuario.FirstOrDefault(u => u.Correo == usuario.Correo);
        
        if (correoExistente != null)
        {
            ModelState.AddModelError("Correo", "El correo ya está registrado");
            return View(usuario);
        }

        //Ajustando Datos
        usuario.Password = Encriptar.EncriptarPassword(usuario.Password);
        usuario.Fecha_Registro = DateTime.Now;
        usuario.Estado_Usuario = true;
        usuario.ID_Tipo_Usuario = 2; //Paciente

        if (usuario.Nombre != "" && usuario.Apellido_Paterno != "" && usuario.Apellido_Materno != "" && usuario.Correo != "" && usuario.CURP != "" && usuario.Password != "")
        {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            var mensaje = string.Format("<h1>Hola</h1> <br /> <p>Se completo correctamente tu registro</p>");
            try
            {
                await _correoElectronico.EnviarCorreo(usuario.Correo, "Registro", mensaje);
            }
            catch (Exception ex)
            {
                return View(usuario);
            }
            ViewBag.Completado += string.Format("<br />Se completo el Registro<br />");
            return View();
        }
        else
        {
            //VER ERROREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEES
            var errors = ModelState.Values.SelectMany(u => u.Errors);
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error.ErrorMessage);
            }
        }

        return View(usuario);
    }

    //Rcuperar Contraseña
    public IActionResult RecuperarPassword()
    {
        ViewBag.Error = String.IsNullOrEmpty(HttpContext.Request.Query["error"]) ? false : true;
        ViewBag.Completo = String.IsNullOrEmpty(HttpContext.Request.Query["completo"]) ? false : true;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RecuperarPassword(string Correo)
    {
        var usuario = _context.Usuario.FirstOrDefault(u => u.Correo == Correo);
        
        if (usuario == null)
        {
            ViewBag.Message += string.Format("<br />Ususario no encontrado<br />");
            return View();
        }

        if (usuario.Estado_Usuario == false)
        {
            ViewBag.Message += string.Format("<br />El usuario no está activo, favor de contactar con el personal<br />");
            return View();
        }

        string password = Utilities.SecurePassword.GenerateSecurePassword(10);

        usuario.Password = Encriptar.EncriptarPassword(password);
        _context.Update(usuario);
        _context.SaveChanges();

        var msg = string.Format("<h1>Hola</h1> <br /> <p>Se ha cambiado tu contraseña a: {0}</p>", password);
        try
        {
            await _correoElectronico.EnviarCorreo(usuario.Correo, "Recuperar Contraseña", msg);
        }
        catch (Exception ex)
        {
            return RedirectToAction("RecuperarPassword", new { error = true });
        }

        ViewBag.Completado += string.Format("<br />Se ha enviado un correo con la nueva contraseña<br />");
        return View();
    }

    //Cerrar Sesión
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction(controllerName: "Home", actionName: "Login");
    }

}
