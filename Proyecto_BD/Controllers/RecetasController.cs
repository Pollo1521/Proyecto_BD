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
    public class RecetasController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public RecetasController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Recetas
        public async Task<IActionResult> Index()
        {
            if(User.IsInRole("3"))
            {
                int id = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);
                var doctor = await _context.Medico.FirstOrDefaultAsync(u => u.ID_Usuario == id);
                if (doctor == null)
                {
                    return NotFound();
                }
                id = doctor.ID_Medico;
                var recetas = await _context.Receta
                    .Include(r => r.Cita)
                    .Where(r => r.Cita.ID_Medico == id)
                    .ToListAsync();

                return View(recetas);
            }

            var contextoBaseDatos = _context.Receta.Include(r => r.Cita);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Recetas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Receta
                .Include(r => r.Cita)
                .FirstOrDefaultAsync(m => m.ID_Receta == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        // GET: Recetas/Create
        public IActionResult Create()
        {
            if (TempData["CitaActual"] is not int idCita)
            {
                return NotFound();
            }
            TempData.Keep("CitaActual");

            ViewData["ID_Cita"] = new SelectList(_context.Cita, "ID_Cita", "ID_Cita", idCita);
            return View();
        }

        // POST: Recetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Receta,ID_Cita,Diagnostico")] Receta receta)
        {
            if (receta.Diagnostico != "")
            {
                _context.Add(receta);
                await _context.SaveChangesAsync();

                TempData["RecetaActual"] = receta.ID_Receta;
                TempData.Keep("RecetaActual");
                return RedirectToAction("RegistrarTratamiento", "Tratamientos");
            }

            return NotFound();
        }

        // GET: Recetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Receta.FindAsync(id);
            if (receta == null)
            {
                return NotFound();
            }
            ViewData["ID_Cita"] = new SelectList(_context.Cita, "ID_Cita", "ID_Cita", receta.ID_Cita);
            return View(receta);
        }

        // POST: Recetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Receta,ID_Cita,Diagnostico")] Receta receta)
        {
            if (id != receta.ID_Receta)
            {
                return NotFound();
            }

            if (receta.Diagnostico != "")
            {
                try
                {
                    _context.Update(receta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecetaExists(receta.ID_Receta))
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
            ViewData["ID_Cita"] = new SelectList(_context.Cita, "ID_Cita", "ID_Cita", receta.ID_Cita);
            return View(receta);
        }

        // GET: Recetas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Receta
                .Include(r => r.Cita)
                .FirstOrDefaultAsync(m => m.ID_Receta == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        // POST: Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receta = await _context.Receta.FindAsync(id);
            if (receta != null)
            {
                _context.Receta.Remove(receta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecetaExists(int id)
        {
            return _context.Receta.Any(e => e.ID_Receta == id);
        }

        public async Task<IActionResult> VerTratamientos(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TempData["RecetaActual"] = id;
            TempData.Keep("RecetaActual");
            return RedirectToAction(controllerName: "Tratamientos", actionName: "TratamientosReceta");
        }
    }
}
