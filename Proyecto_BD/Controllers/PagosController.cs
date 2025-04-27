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
    public class PagosController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public PagosController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Pagos
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Pago.Include(p => p.Cita);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Pagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pago
                .Include(p => p.Cita)
                .FirstOrDefaultAsync(m => m.ID_Pago == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagos/Create
        public IActionResult Create()
        {
            ViewData["ID_Cita"] = new SelectList(_context.Cita, "ID_Cita", "ID_Cita");
            return View();
        }

        // POST: Pagos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Pago,ID_Cita,Estado_Pago,ComprobantePago")] Pago pago)
        {
            if (pago.ComprobantePago != "")
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Cita"] = new SelectList(_context.Cita, "ID_Cita", "ID_Cita", pago.ID_Cita);
            return View(pago);
        }

        // GET: Pagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pago.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["ID_Cita"] = new SelectList(_context.Cita, "ID_Cita", "ID_Cita", pago.ID_Cita);
            return View(pago);
        }

        // POST: Pagos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Pago,ID_Cita,Estado_Pago,ComprobantePago")] Pago pago)
        {
            if (id != pago.ID_Pago)
            {
                return NotFound();
            }

            if (pago.ComprobantePago != "")
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.ID_Pago))
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
            ViewData["ID_Cita"] = new SelectList(_context.Cita, "ID_Cita", "ID_Cita", pago.ID_Cita);
            return View(pago);
        }

        // GET: Pagos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pago
                .Include(p => p.Cita)
                .FirstOrDefaultAsync(m => m.ID_Pago == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pago.FindAsync(id);
            if (pago != null)
            {
                _context.Pago.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.Pago.Any(e => e.ID_Pago == id);
        }
    }
}
