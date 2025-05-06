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
    public class PacientesController : Controller
    {
        private readonly ContextoBaseDatos _context;
        private readonly CorreoElectronico _correoElectronico;

        public PacientesController(ContextoBaseDatos context, CorreoElectronico correoElectronico)
        {
            _context = context;
            _correoElectronico = correoElectronico;
        }

        // GET: Pacientes
        [Authorize(Roles = "1, 2, 4")]
        public async Task<IActionResult> Index()
        {
            // Verifica si el usuario tiene el rol de administrador o Recepcionista
            if (User.IsInRole("1") || User.IsInRole("4"))
            {
                var contextoBaseDatos = _context.Paciente.Include(p => p.Tipo_Sangres).Include(p => p.Usuario);
                return View(await contextoBaseDatos.ToListAsync());
            }

            // Si el usuario no es administrador o Recepcionista, redirige a la vista de detalles del paciente
            int ID_Usuario = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);
            var paciente = await _context.Paciente
                .Include(p => p.Tipo_Sangres)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.ID_Usuario == ID_Usuario);

            if (paciente == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = paciente.ID_Paciente });

        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente
                .Include(p => p.Tipo_Sangres)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.ID_Paciente == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        [Authorize(Roles = "1, 2")]
        public IActionResult Create()
        {
            ViewData["ID_Tipo_Sangre"] = new SelectList(_context.TipoSangre, "ID_Tipo_Sangre", "Tipo_Sangre");
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Paciente,ID_Tipo_Sangre,Peso,Alergia,Estatura")] Paciente paciente)
        {
            int ID_Usuario = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);
            paciente.ID_Usuario = ID_Usuario;

            if (paciente.Peso != 0 && paciente.Alergia != "" && paciente.Estatura != 0)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Tipo_Sangre"] = new SelectList(_context.TipoSangre, "ID_Tipo_Sangre", "Tipo_Sangre", paciente.ID_Tipo_Sangre);
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario, "ID_Usuario", "Apellido_Materno", paciente.ID_Usuario);
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = _context.Paciente.Include(p => p.Usuario).FirstOrDefault(p => p.ID_Paciente == id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["ID_Tipo_Sangre"] = new SelectList(_context.TipoSangre, "ID_Tipo_Sangre", "Tipo_Sangre", paciente.ID_Tipo_Sangre);
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Paciente,ID_Usuario,ID_Tipo_Sangre,Peso,Alergia,Estatura")] Paciente paciente, 
                                              string Nombre, string ApPa, string ApMa, string Correo, DateTime FechaNacimiento)
        {
            if (id != paciente.ID_Paciente)
            {
                return NotFound();
            }

            if (paciente.Peso != 0 && paciente.Alergia != "" && paciente.Estatura != 0)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();

                    var usuario = await _context.Usuario.FindAsync(paciente.ID_Usuario);

                    if (usuario != null)
                    {
                        // Verifica si el correo ya existe
                        var correoExistente = _context.Usuario.FirstOrDefault(u => u.Correo == Correo && u.ID_Usuario != usuario.ID_Usuario);
                        if (correoExistente != null)
                        {
                            ViewBag.Error += string.Format("El correo ya esta registrado<br />");                            
                            ViewData["ID_Tipo_Sangre"] = new SelectList(_context.TipoSangre, "ID_Tipo_Sangre", "Tipo_Sangre", paciente.ID_Tipo_Sangre);
                            return View(paciente);
                        }

                        // Verifica si el correo ha cambiado
                        if (Correo != usuario.Correo)
                        {
                            try
                            {
                                await _correoElectronico.EnviarCorreo(usuario.Correo, "Cambio de Correo", "Se ha realizado un cambio de correo");
                            }
                            catch (Exception ex)
                            {
                                ViewData["ID_Tipo_Sangre"] = new SelectList(_context.TipoSangre, "ID_Tipo_Sangre", "Tipo_Sangre", paciente.ID_Tipo_Sangre);
                                return View(paciente);
                            }
                        }

                        usuario.Nombre = Nombre;
                        usuario.Apellido_Paterno = ApPa;
                        usuario.Apellido_Materno = ApMa;
                        usuario.Correo = Correo;
                        usuario.Fecha_Nacimiento = FechaNacimiento;
                        _context.Update(usuario);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.ID_Paciente))
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
            ViewData["ID_Tipo_Sangre"] = new SelectList(_context.TipoSangre, "ID_Tipo_Sangre", "Tipo_Sangre", paciente.ID_Tipo_Sangre);
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Paciente
                .Include(p => p.Tipo_Sangres)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.ID_Paciente == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Paciente.FindAsync(id);
            if (paciente != null)
            {
                _context.Paciente.Remove(paciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Paciente.Any(e => e.ID_Paciente == id);
        }
    }
}
