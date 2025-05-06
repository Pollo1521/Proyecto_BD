using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_BD.Models;
using Proyecto_BD.Utilities;

namespace Proyecto_BD.Controllers
{
    public class MedicosController : Controller
    {
        private readonly ContextoBaseDatos _context;
        private readonly CorreoElectronico _correoElectronico;

        public MedicosController(ContextoBaseDatos context, CorreoElectronico correoElectronico)
        {
            _context = context;
            _correoElectronico = correoElectronico;
        }

        // GET: Medicos
        [Authorize(Roles = "1, 3, 4")]
        public async Task<IActionResult> Index()
        {
            // Verifica si el usuario tiene el rol de administrador o Recepcionista
            if (User.IsInRole("1") || User.IsInRole("4"))
            {
                var contextoBaseDatos = _context.Medico.Include(m => m.Consultorio).Include(m => m.Especialidad).Include(m => m.Jornada).Include(m => m.Usuario);
                return View(await contextoBaseDatos.ToListAsync());
            }

            int ID_Usuario = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);

            var medico = await _context.Medico
                        .Include(m => m.Consultorio)
                        .Include(m => m.Especialidad)
                        .Include(m => m.Jornada)
                        .Include(m => m.Usuario)
                        .FirstOrDefaultAsync(m => m.ID_Usuario == ID_Usuario);

            if (medico == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = medico.ID_Medico });
        }

        // GET: Medicos/Details/5
        [Authorize(Roles = "1, 3, 4")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medico
                .Include(m => m.Consultorio)
                .Include(m => m.Especialidad)
                .Include(m => m.Jornada)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.ID_Medico == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // GET: Medicos/Create
        [Authorize(Roles = "1, 4")]
        public IActionResult Create(int id)
        {
            ViewData["ID_Consultorio"] = new SelectList(_context.Consultorio, "ID_Consultorio", "Numero_Consultorio");
            ViewData["ID_Especialidad"] = new SelectList(_context.Especialidad, "ID_Especialidad", "Descripcion");
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "descripcion");
            ViewData["ID_Usuario"] = id;
            return View();
        }

        // POST: Medicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Medico,ID_Usuario,Cedula,ID_Especialidad,ID_Consultorio,ID_Jornada")] Medico medico)
        {
            if (medico.Cedula != "")
            {
                _context.Add(medico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Consultorio"] = new SelectList(_context.Consultorio, "ID_Consultorio", "Numero_Consultorio", medico.ID_Consultorio);
            ViewData["ID_Especialidad"] = new SelectList(_context.Especialidad, "ID_Especialidad", "Descripcion", medico.ID_Especialidad);
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "descripcion", medico.ID_Jornada);
            ViewData["ID_Usuario"] = medico.ID_Usuario;
            return View(medico);
        }

        // GET: Medicos/Edit/5
        [Authorize(Roles = "1, 3, 4")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medico.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            ViewData["ID_Consultorio"] = new SelectList(_context.Consultorio, "ID_Consultorio", "Numero_Consultorio", medico.ID_Consultorio);
            ViewData["ID_Especialidad"] = new SelectList(_context.Especialidad, "ID_Especialidad", "Descripcion", medico.ID_Especialidad);
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "descripcion", medico.ID_Jornada);
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario, "ID_Usuario", "Nombre", medico.ID_Usuario);
            return View(medico);
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Medico,ID_Usuario,Cedula,ID_Especialidad,ID_Consultorio,ID_Jornada")] Medico medico)
        {
            if (id != medico.ID_Medico)
            {
                return NotFound();
            }

            if (medico.Cedula != "")
            {
                try
                {
                    _context.Update(medico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.ID_Medico))
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
            ViewData["ID_Consultorio"] = new SelectList(_context.Consultorio, "ID_Consultorio", "Numero_Consultorio", medico.ID_Consultorio);
            ViewData["ID_Especialidad"] = new SelectList(_context.Especialidad, "ID_Especialidad", "Descripcion", medico.ID_Especialidad);
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "descripcion", medico.ID_Jornada);
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario, "ID_Usuario", "Nombre", medico.ID_Usuario);
            return View(medico);
        }

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medico
                .Include(m => m.Consultorio)
                .Include(m => m.Especialidad)
                .Include(m => m.Jornada)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.ID_Medico == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medico = await _context.Medico.FindAsync(id);
            if (medico != null)
            {
                _context.Medico.Remove(medico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicoExists(int id)
        {
            return _context.Medico.Any(e => e.ID_Medico == id);
        }


        // GET: Medicos/ModificarDAtosPersonales/5

        [Authorize(Roles = "1, 3, 4")]
        public async Task<IActionResult> EditarDatos()
        {

            //Obtener el ID del usuario desde el token JWT
            int id = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);

            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            ViewData["ID_Tipo_Usuario"] = usuario.ID_Tipo_Usuario;
            return View(usuario);
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarDatos(int id, [Bind("ID_Usuario,Nombre,Apellido_Paterno,Apellido_Materno,Correo,CURP,Fecha_Nacimiento,Password,Fecha_Registro,ID_Tipo_Usuario,Estado_Usuario")] Usuario usuario)
        {
            if (id != usuario.ID_Usuario)
            {
                return NotFound();
            }

            // Verifica si el correo ya existe
            var correoExistente = _context.Usuario.FirstOrDefault(u => u.Correo == usuario.Correo && u.ID_Usuario != usuario.ID_Usuario);

            if (correoExistente != null)
            {
                ViewBag.Error += string.Format("El correo ya esta registrado<br />");
                ViewData["ID_Tipo_Usuario"] = usuario.ID_Tipo_Usuario;
                return View(usuario);
            }

            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                await _correoElectronico.EnviarCorreo(usuario.Correo, "Cambio de Datos", "Se ha realizado un cambio de datos personales");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicoExists(usuario.ID_Usuario))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
