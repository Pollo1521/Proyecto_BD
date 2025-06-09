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
    public class CitasController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public CitasController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Cita.Include(c => c.CitasHorario).Include(c => c.EstatusCita).Include(c => c.Medico).Include(c => c.Paciente);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .Include(c => c.CitasHorario)
                .Include(c => c.EstatusCita)
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.ID_Cita == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            ViewData["ID_Cita_Horario"] = new SelectList(_context.CitasHorario, "ID_Horario", "Hora_Cita");
            ViewData["ID_Estatus_Cita"] = new SelectList(_context.EstatusCita, "ID_Estatus_Cita", "Estatus_Cita");
            ViewData["ID_Medico"] = new SelectList(_context.Medico, "ID_Medico", "Cedula");
            ViewData["ID_Paciente"] = new SelectList(_context.Paciente, "ID_Paciente", "Alergia");
            return View();
        }

        // POST: Citas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Cita,ID_Paciente,ID_Medico,Fecha_Registro,Fecha_Cita,ID_Cita_Horario,ID_Estatus_Cita")] Cita cita)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Cita_Horario"] = new SelectList(_context.CitasHorario, "ID_Horario", "ID_Horario", cita.ID_Cita_Horario);
            ViewData["ID_Estatus_Cita"] = new SelectList(_context.EstatusCita, "ID_Estatus_Cita", "Estatus_Cita", cita.ID_Estatus_Cita);
            ViewData["ID_Medico"] = new SelectList(_context.Medico, "ID_Medico", "Cedula", cita.ID_Medico);
            ViewData["ID_Paciente"] = new SelectList(_context.Paciente, "ID_Paciente", "Alergia", cita.ID_Paciente);
            return View(cita);
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            ViewData["ID_Cita_Horario"] = new SelectList(_context.CitasHorario, "ID_Horario", "ID_Horario", cita.ID_Cita_Horario);
            ViewData["ID_Estatus_Cita"] = new SelectList(_context.EstatusCita, "ID_Estatus_Cita", "Estatus_Cita", cita.ID_Estatus_Cita);
            ViewData["ID_Medico"] = new SelectList(_context.Medico, "ID_Medico", "Cedula", cita.ID_Medico);
            ViewData["ID_Paciente"] = new SelectList(_context.Paciente, "ID_Paciente", "Alergia", cita.ID_Paciente);
            return View(cita);
        }

        // POST: Citas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Cita,ID_Paciente,ID_Medico,Fecha_Registro,Fecha_Cita,ID_Cita_Horario,ID_Estatus_Cita")] Cita cita)
        {
            if (id != cita.ID_Cita)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.ID_Cita))
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
            ViewData["ID_Cita_Horario"] = new SelectList(_context.CitasHorario, "ID_Horario", "ID_Horario", cita.ID_Cita_Horario);
            ViewData["ID_Estatus_Cita"] = new SelectList(_context.EstatusCita, "ID_Estatus_Cita", "Estatus_Cita", cita.ID_Estatus_Cita);
            ViewData["ID_Medico"] = new SelectList(_context.Medico, "ID_Medico", "Cedula", cita.ID_Medico);
            ViewData["ID_Paciente"] = new SelectList(_context.Paciente, "ID_Paciente", "Alergia", cita.ID_Paciente);
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .Include(c => c.CitasHorario)
                .Include(c => c.EstatusCita)
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.ID_Cita == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cita = await _context.Cita.FindAsync(id);
            if (cita != null)
            {
                _context.Cita.Remove(cita);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
            return _context.Cita.Any(e => e.ID_Cita == id);
        }
    }
}
