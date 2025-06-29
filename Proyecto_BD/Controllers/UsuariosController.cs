using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Proyecto_BD.Models;
using Proyecto_BD.Utilities;

namespace Proyecto_BD.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ContextoBaseDatos _context;
        private readonly CorreoElectronico _correoElectronico;

        public UsuariosController(ContextoBaseDatos context, CorreoElectronico correoElectronico)
        {
            _context = context;
            _correoElectronico = correoElectronico;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Usuario.Include(u => u.TipoUsuario);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(m => m.ID_Usuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        [Authorize(Roles = "1, 4")]
        public IActionResult Create()
        {
            var tiposUsuario = new[] { 3, 4 };

            ViewData["ID_Tipo_Usuario"] = new SelectList(_context.TiposUsuario.Where(t => tiposUsuario.Contains(t.ID_Tipo_Usuario)), "ID_Tipo_Usuario", "Tipo_Usuario");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Usuario,Nombre,Apellido_Paterno,Apellido_Materno,Correo,CURP,Fecha_Nacimiento,ID_Tipo_Usuario")] Usuario usuario)
        {
            var tiposUsuario = new[] { 3, 4 };
            //Validando si el correo ya existe
            var correoExistente = _context.Usuario.FirstOrDefault(u => u.Correo == usuario.Correo);

            if (correoExistente != null)
            {
                ModelState.AddModelError("Correo", "El correo ya está registrado");
                
                tiposUsuario = new[] { 3, 4 };

                ViewData["ID_Tipo_Usuario"] = new SelectList(_context.TiposUsuario.Where(t => tiposUsuario.Contains(t.ID_Tipo_Usuario)), "ID_Tipo_Usuario", "Tipo_Usuario", usuario.ID_Tipo_Usuario);
                return View(usuario);
            }

            //Ajustando Datos
            string password = SecurePassword.GenerateSecurePassword(10);
            usuario.Password = Encriptar.EncriptarPassword(password);
            usuario.Fecha_Registro = DateTime.Now;
            usuario.Estado_Usuario = true;

            
            if (usuario.Nombre != "" && usuario.Apellido_Paterno != "" && usuario.Apellido_Materno != "" && usuario.Correo != "" && usuario.CURP != "")
            {
                //Registro Doctor
                if (usuario.ID_Tipo_Usuario == 3)
                {
                    var mensaje = string.Format("<h1>Bienvenido, Doctor</h1> <br /> <p>Se completo correctamente tu registro. Su contraseña asignada es: {0}</p>", password);
                    try
                    {
                        await _correoElectronico.EnviarCorreo(usuario.Correo, "Registro", mensaje);
                        _context.Add(usuario);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Create", "Medicos", new {id = usuario.ID_Usuario});
                    }
                    catch (Exception ex)
                    {
                        //Holi, no se que poner aqui XD
                    }
                }

                //Registro Recepcionista
                if (usuario.ID_Tipo_Usuario == 4)
                {
                    var mensaje = string.Format("<h1>Bienvenido, Recepcionista</h1> <br /> <p>Se completo correctamente tu registro. Su contraseña asignada es: {0}</p>", password);
                    try
                    {
                        await _correoElectronico.EnviarCorreo(usuario.Correo, "Registro", mensaje);
                        _context.Add(usuario);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Create", "Recepcionistas", new { id = usuario.ID_Usuario });
                    }
                    catch (Exception ex)
                    {
                        //Holi, no se que poner aqui XD
                    }
                }
            }

            ViewData["ID_Tipo_Usuario"] = new SelectList(_context.TiposUsuario.Where(t => tiposUsuario.Contains(t.ID_Tipo_Usuario)), "ID_Tipo_Usuario", "Tipo_Usuario", usuario.ID_Tipo_Usuario);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["ID_Tipo_Usuario"] = new SelectList(_context.TiposUsuario, "ID_Tipo_Usuario", "Tipo_Usuario", usuario.ID_Tipo_Usuario);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Usuario,Nombre,Apellido_Paterno,Apellido_Materno,Correo,CURP,Fecha_Nacimiento,Password,Fecha_Registro,ID_Tipo_Usuario,Estado_Usuario")] Usuario usuario)
        {
            if (id != usuario.ID_Usuario)
            {
                return NotFound();
            }

            if (usuario.Nombre != "" && usuario.Apellido_Paterno != "" && usuario.Apellido_Materno != "" && usuario.Correo != "" && usuario.CURP != "" && usuario.Password != "")
            {
                try
                {
                    //usuario.Password = Encriptar.EncriptarPassword(usuario.Password);
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.ID_Usuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Tipo_Usuario"] = new SelectList(_context.TiposUsuario, "ID_Tipo_Usuario", "Tipo_Usuario", usuario.ID_Tipo_Usuario);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(m => m.ID_Usuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.ID_Usuario == id);
        }        
    }
}
