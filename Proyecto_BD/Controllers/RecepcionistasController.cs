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
    public class RecepcionistasController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public RecepcionistasController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Recepcionistas
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Recepcionista.Include(r => r.Jornada).Include(r => r.Usuario);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Recepcionistas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionista
                .Include(r => r.Jornada)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.ID_Recepcionista == id);
            if (recepcionista == null)
            {
                return NotFound();
            }

            return View(recepcionista);
        }

        // GET: Recepcionistas/Create
        public IActionResult Create()
        {
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada");
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario.Where(s => s.ID_Tipo_Usuario == 4), "ID_Usuario", "Apellido_Materno");
            return View();
        }

        // POST: Recepcionistas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Recepcionista,ID_Usuario,ID_Jornada")] Recepcionista recepcionista)
        {
            if (recepcionista.ID_Usuario != 0 && recepcionista.ID_Jornada != 0)
            {
                _context.Add(recepcionista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada", recepcionista.ID_Jornada);
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario.Where(s => s.ID_Tipo_Usuario == 1), "ID_Usuario", "Nombre", recepcionista.ID_Usuario);
            return View(recepcionista);
        }

        // GET: Recepcionistas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionista.FindAsync(id);
            if (recepcionista == null)
            {
                return NotFound();
            }
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada", recepcionista.ID_Jornada);
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario, "ID_Usuario", "Apellido_Materno", recepcionista.ID_Usuario);
            return View(recepcionista);
        }

        // POST: Recepcionistas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Recepcionista,ID_Usuario,ID_Jornada")] Recepcionista recepcionista)
        {
            if (id != recepcionista.ID_Recepcionista)
            {
                return NotFound();
            }

            if (recepcionista.ID_Usuario != 0 && recepcionista.ID_Jornada != 0)
            {
                try
                {
                    _context.Update(recepcionista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecepcionistaExists(recepcionista.ID_Recepcionista))
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
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada", recepcionista.ID_Jornada);
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario, "ID_Usuario", "Apellido_Materno", recepcionista.ID_Usuario);
            return View(recepcionista);
        }

        // GET: Recepcionistas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionista
                .Include(r => r.Jornada)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.ID_Recepcionista == id);
            if (recepcionista == null)
            {
                return NotFound();
            }

            return View(recepcionista);
        }

        // POST: Recepcionistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recepcionista = await _context.Recepcionista.FindAsync(id);
            if (recepcionista != null)
            {
                _context.Recepcionista.Remove(recepcionista);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecepcionistaExists(int id)
        {
            return _context.Recepcionista.Any(e => e.ID_Recepcionista == id);
        }
    }
}
