using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Create()
        {
            ViewData["ID_Tipo_Usuario"] = new SelectList(_context.TiposUsuario, "ID_Tipo_Usuario", "Tipo_Usuario");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Usuario,Nombre,Apellido_Paterno,Apellido_Materno,Correo,CURP,Fecha_Nacimiento,Password,Fecha_Registro,ID_Tipo_Usuario,Estado_Usuario")] Usuario usuario)
        {
            //Ajustando Datos
            usuario.Password = Encriptar.EncriptarPassword(usuario.Password);
            usuario.Fecha_Registro = DateTime.Now;
            usuario.Estado_Usuario = true;

            if (usuario.Nombre != "" && usuario.Apellido_Paterno != "" && usuario.Apellido_Materno != "" && usuario.Correo != "" && usuario.CURP != "" && usuario.Password != "")
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                //await _correoElectronico.EnviarCorreo(usuario.Correo, "Registro", "Usuario Registrado con exito");
                return RedirectToAction(nameof(Index));
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

            ViewData["ID_Tipo_Usuario"] = new SelectList(_context.TiposUsuario, "ID_Tipo_Usuario", "Tipo_Usuario", usuario.ID_Tipo_Usuario);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
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
