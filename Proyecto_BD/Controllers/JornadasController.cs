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
    public class JornadasController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public JornadasController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Jornadas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Jornada.ToListAsync());
        }

        // GET: Jornadas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jornada = await _context.Jornada
                .FirstOrDefaultAsync(m => m.ID_Jornada == id);
            if (jornada == null)
            {
                return NotFound();
            }

            return View(jornada);
        }

        // GET: Jornadas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jornadas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Jornada,Hora_Entrada,Hora_Salida")] Jornada jornada)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jornada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jornada);
        }

        // GET: Jornadas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jornada = await _context.Jornada.FindAsync(id);
            if (jornada == null)
            {
                return NotFound();
            }
            return View(jornada);
        }

        // POST: Jornadas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Jornada,Hora_Entrada,Hora_Salida")] Jornada jornada)
        {
            if (id != jornada.ID_Jornada)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jornada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JornadaExists(jornada.ID_Jornada))
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
            return View(jornada);
        }

        // GET: Jornadas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jornada = await _context.Jornada
                .FirstOrDefaultAsync(m => m.ID_Jornada == id);
            if (jornada == null)
            {
                return NotFound();
            }

            return View(jornada);
        }

        // POST: Jornadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jornada = await _context.Jornada.FindAsync(id);
            if (jornada != null)
            {
                _context.Jornada.Remove(jornada);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JornadaExists(int id)
        {
            return _context.Jornada.Any(e => e.ID_Jornada == id);
        }
    }
}
