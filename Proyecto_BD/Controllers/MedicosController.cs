using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_BD.Models;

namespace Proyecto_BD.Controllers
{
    public class MedicosController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public MedicosController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Medico.Include(m => m.Consultorio).Include(m => m.Especialidad).Include(m => m.Jornada).Include(m => m.Usuario);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Medicos/Details/5
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
        public IActionResult Create()
        {
            ViewData["ID_Consultorio"] = new SelectList(_context.Consultorio, "ID_Consultorio", "Numero_Consultorio");
            ViewData["ID_Especialidad"] = new SelectList(_context.Especialidad, "ID_Especialidad", "Descripcion");
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada");
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario, "ID_Usuario", "Nombre");
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
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada", medico.ID_Jornada);
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario, "ID_Usuario", "Apellido_Materno", medico.ID_Usuario);
            return View(medico);
        }

        // GET: Medicos/Edit/5
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
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada", medico.ID_Jornada);
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
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada", medico.ID_Jornada);
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
    }
}
