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
            // Verifica si el usuario tiene el rol de administrador o Recepcionista
            if (User.IsInRole("2"))
            {
                return RedirectToAction(controllerName: "Pacientes", actionName: "Index");
            }

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

        public IActionResult RealizarPago()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RealizarPago(string folio)
        {
            if (string.IsNullOrEmpty(folio))
            {
                TempData["Error"] = "El folio no puede estar vacío.";
                return View();
            }

            if (!int.TryParse(folio, out int idCita))
            {
                TempData["Error"] = "El folio debe ser un número válido.";
                return View();
            }

            var pago = await _context.Pago.Include(p => p.Cita).FirstOrDefaultAsync(p => p.ID_Cita == idCita);
            if (pago == null)
            {
                TempData["Error"] = "No se encontró un pago con el folio proporcionado.";
                return View();
            }

            var fechaRegistro = pago.Cita.Fecha_Registro;
            var tiempoLimite = fechaRegistro.AddHours(8);

            if (DateTime.Now > tiempoLimite)
            {
                TempData["Error"] = "El tiempo para realizar el pago ha expirado.";
                return View();
            }

            if (pago.Estado_Pago)
            {
                TempData["Error"] = "El pago ya ha sido realizado.";
                return View();
            }

            pago.Estado_Pago = true;
            pago.ComprobantePago = "Ya pagaron";

            _context.Update(pago);
            await _context.SaveChangesAsync();

            var cita = await _context.Cita.FirstOrDefaultAsync(c => c.ID_Cita == idCita);

            if (cita != null)
            {
                cita.ID_Estatus_Cita = 2;
                _context.Update(cita);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Pago realizado exitosamente.";
            return View();
        }
    }
}