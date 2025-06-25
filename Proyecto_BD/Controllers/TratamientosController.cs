using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_BD.Models;

namespace Proyecto_BD.Controllers
{
    public class TratamientosController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public TratamientosController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Tratamientos
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Tratamiento.Include(t => t.Receta);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Tratamientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamiento
                .Include(t => t.Receta)
                .FirstOrDefaultAsync(m => m.ID_Tratamiento == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // GET: Tratamientos/Create
        public IActionResult Create()
        {
            ViewData["ID_Receta"] = new SelectList(_context.Receta, "ID_Receta", "ID_Receta");
            return View();
        }

        // POST: Tratamientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Tratamiento,ID_Receta,Medicamento,Indicaciones")] Tratamiento tratamiento)
        {
            if (tratamiento.Medicamento != "" && tratamiento.Indicaciones != "")
            {
                _context.Add(tratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Receta"] = new SelectList(_context.Receta, "ID_Receta", "ID_Receta", tratamiento.ID_Receta);
            return View(tratamiento);
        }

        // GET: Tratamientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamiento.FindAsync(id);
            if (tratamiento == null)
            {
                return NotFound();
            }
            ViewData["ID_Receta"] = new SelectList(_context.Receta, "ID_Receta", "Diagnostico", tratamiento.ID_Receta);
            return View(tratamiento);
        }

        // POST: Tratamientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Tratamiento,ID_Receta,Medicamento,Indicaciones")] Tratamiento tratamiento)
        {
            if (id != tratamiento.ID_Tratamiento)
            {
                return NotFound();
            }

            if (tratamiento.Medicamento != "" && tratamiento.Indicaciones != "")
            {
                try
                {
                    _context.Update(tratamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TratamientoExists(tratamiento.ID_Tratamiento))
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
            ViewData["ID_Receta"] = new SelectList(_context.Receta, "ID_Receta", "Diagnostico", tratamiento.ID_Receta);
            return View(tratamiento);
        }

        // GET: Tratamientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamiento
                .Include(t => t.Receta)
                .FirstOrDefaultAsync(m => m.ID_Tratamiento == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // POST: Tratamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tratamiento = await _context.Tratamiento.FindAsync(id);
            if (tratamiento != null)
            {
                _context.Tratamiento.Remove(tratamiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TratamientoExists(int id)
        {
            return _context.Tratamiento.Any(e => e.ID_Tratamiento == id);
        }

        [Authorize(Roles = "1, 4")]
        public async Task<IActionResult> RegistrarTratamiento()
        {
            if (TempData["RecetaActual"] is not int idReceta)
            {
                return NotFound();
            }
            TempData.Keep("RecetaActual");

            var tratamientos = await _context.Tratamiento.Include(t => t.ID_Receta).Where(t => t.ID_Receta == idReceta).ToListAsync();

            var view = new List<Tratamiento>();

            foreach(var tratamiento in tratamientos)
            {
                view.Add(new Tratamiento
                {
                    ID_Receta = tratamiento.ID_Receta,
                    Medicamento = tratamiento.Medicamento,
                    Indicaciones = tratamiento.Indicaciones
                });
            }

            return View(view);
        }
    }
}
