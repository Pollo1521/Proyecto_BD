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
    public class VentasController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public VentasController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Venta.Include(v => v.Recepcionista);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.Venta
                .Include(v => v.Recepcionista)
                .FirstOrDefaultAsync(m => m.ID_Ventas == id);
            if (ventas == null)
            {
                return NotFound();
            }

            return View(ventas);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["ID_Recepcionista"] = new SelectList(_context.Recepcionista, "ID_Recepcionista", "ID_Recepcionista");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Ventas,Fecha_Venta,ID_Recepcionista")] Ventas ventas)
        {
            if (ventas.ID_Recepcionista != 0)
            {
                _context.Add(ventas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Recepcionista"] = new SelectList(_context.Recepcionista, "ID_Recepcionista", "ID_Recepcionista", ventas.ID_Recepcionista);
            return View(ventas);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.Venta.FindAsync(id);
            if (ventas == null)
            {
                return NotFound();
            }
            ViewData["ID_Recepcionista"] = new SelectList(_context.Recepcionista, "ID_Recepcionista", "ID_Recepcionista", ventas.ID_Recepcionista);
            return View(ventas);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Ventas,Fecha_Venta,ID_Recepcionista")] Ventas ventas)
        {
            if (id != ventas.ID_Ventas)
            {
                return NotFound();
            }

            if (ventas.ID_Recepcionista != 0)
            {
                try
                {
                    _context.Update(ventas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentasExists(ventas.ID_Ventas))
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
            ViewData["ID_Recepcionista"] = new SelectList(_context.Recepcionista, "ID_Recepcionista", "ID_Recepcionista", ventas.ID_Recepcionista);
            return View(ventas);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.Venta
                .Include(v => v.Recepcionista)
                .FirstOrDefaultAsync(m => m.ID_Ventas == id);
            if (ventas == null)
            {
                return NotFound();
            }

            return View(ventas);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ventas = await _context.Venta.FindAsync(id);
            if (ventas != null)
            {
                _context.Venta.Remove(ventas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentasExists(int id)
        {
            return _context.Venta.Any(e => e.ID_Ventas == id);
        }
    }
}
