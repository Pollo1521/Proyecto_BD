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
    public class TicketsController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public TicketsController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Ticket.Include(t => t.Medicina).Include(t => t.Servicio).Include(t => t.Venta);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Medicina)
                .Include(t => t.Servicio)
                .Include(t => t.Venta)
                .FirstOrDefaultAsync(m => m.ID_Ticket == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["ID_Medicina"] = new SelectList(_context.Medicina, "ID_Medicina", "Nombre_Medicina");
            ViewData["ID_Servicio"] = new SelectList(_context.Servicio, "ID_Servicio", "Descripcion");
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Ticket,ID_Venta,ID_Medicina,ID_Servicio")] Ticket ticket)
        {
            if (ticket.ID_Medicina != 0 || ticket.ID_Servicio != 0)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Medicina"] = new SelectList(_context.Medicina, "ID_Medicina", "Nombre_Medicina", ticket.ID_Medicina);
            ViewData["ID_Servicio"] = new SelectList(_context.Servicio, "ID_Servicio", "Descripcion", ticket.ID_Servicio);
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas", ticket.ID_Venta);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ID_Medicina"] = new SelectList(_context.Medicina, "ID_Medicina", "Nombre_Medicina", ticket.ID_Medicina);
            ViewData["ID_Servicio"] = new SelectList(_context.Servicio, "ID_Servicio", "Descripcion", ticket.ID_Servicio);
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas", ticket.ID_Venta);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Ticket,ID_Venta,ID_Medicina,ID_Servicio")] Ticket ticket)
        {
            if (id != ticket.ID_Ticket)
            {
                return NotFound();
            }

            if (ticket.ID_Medicina != 0 || ticket.ID_Servicio != 0)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.ID_Ticket))
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
            ViewData["ID_Medicina"] = new SelectList(_context.Medicina, "ID_Medicina", "Nombre_Medicina", ticket.ID_Medicina);
            ViewData["ID_Servicio"] = new SelectList(_context.Servicio, "ID_Servicio", "Descripcion", ticket.ID_Servicio);
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas", ticket.ID_Venta);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Medicina)
                .Include(t => t.Servicio)
                .Include(t => t.Venta)
                .FirstOrDefaultAsync(m => m.ID_Ticket == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.ID_Ticket == id);
        }
    }
}
