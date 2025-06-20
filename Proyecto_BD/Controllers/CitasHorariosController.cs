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
    public class CitasHorariosController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public CitasHorariosController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: CitasHorarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.CitasHorario.ToListAsync());
        }

        // GET: CitasHorarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citasHorario = await _context.CitasHorario
                .FirstOrDefaultAsync(m => m.ID_Horario == id);
            if (citasHorario == null)
            {
                return NotFound();
            }

            return View(citasHorario);
        }

        // GET: CitasHorarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CitasHorarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Horario,Hora_Cita,JornadaHorario")] CitasHorario citasHorario)
        {
            if (citasHorario.Hora_Cita != null)
            {
                _context.Add(citasHorario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(citasHorario);
        }

        // GET: CitasHorarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citasHorario = await _context.CitasHorario.FindAsync(id);
            if (citasHorario == null)
            {
                return NotFound();
            }
            return View(citasHorario);
        }

        // POST: CitasHorarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Horario,Hora_Cita,JornadaHorario")] CitasHorario citasHorario)
        {
            if (id != citasHorario.ID_Horario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citasHorario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitasHorarioExists(citasHorario.ID_Horario))
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
            return View(citasHorario);
        }

        // GET: CitasHorarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citasHorario = await _context.CitasHorario
                .FirstOrDefaultAsync(m => m.ID_Horario == id);
            if (citasHorario == null)
            {
                return NotFound();
            }

            return View(citasHorario);
        }

        // POST: CitasHorarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var citasHorario = await _context.CitasHorario.FindAsync(id);
            if (citasHorario != null)
            {
                _context.CitasHorario.Remove(citasHorario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitasHorarioExists(int id)
        {
            return _context.CitasHorario.Any(e => e.ID_Horario == id);
        }
    }
}
