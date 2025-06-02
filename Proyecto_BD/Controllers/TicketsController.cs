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
            var tickets = await _context.Ticket.Include(t => t.Venta).ToListAsync();

            var view = new List<Ticket>();

            foreach (var ticket in tickets)
            {
                object? value = ticket.Tipo_item

                    ? _context.Medicina.FirstOrDefault(s => s.ID_Medicina == ticket.ID_Item)
                    : _context.Servicio.FirstOrDefault(p => p.ID_Servicio == ticket.ID_Item);

                view.Add(new Ticket
                {
                    ID_Ticket = ticket.ID_Ticket,
                    ID_Venta = ticket.ID_Venta,
                    Tipo_item = ticket.Tipo_item,
                    ID_Item = ticket.ID_Item,
                    Venta = ticket.Venta,
                    Medicina = ticket.Tipo_item ? value as Medicina : null,  // Asigna null si no es Medicina
                    Servicio = !ticket.Tipo_item ? value as Servicio : null,  // Asigna null si no es Servicio
                    Cantidad = ticket.Cantidad,
                    Subtotal = ticket.Subtotal
                });
            }

            return View(view);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Venta)
                .FirstOrDefaultAsync(m => m.ID_Ticket == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        // Agregar un servicio
        public IActionResult AgregarServicio()
        {
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas");
            ViewData["ID_Servicio"] = new SelectList(_context.Servicio, "ID_Servicio", "Descripcion");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarServicio([Bind("ID_Ticket,ID_Venta,Tipo_item,ID_Item,Cantidad,Subtotal")] Ticket ticket)
        {
            if (ticket.ID_Venta != 0 && ticket.ID_Item != 0)
            {
                ticket.Tipo_item = false;

                var precio = ticket.Tipo_item
                    ? _context.Medicina.Where(m => m.ID_Medicina == ticket.ID_Item).Select(m => m.Precio_Medicina).FirstOrDefault()
                    : _context.Servicio.Where(s => s.ID_Servicio == ticket.ID_Item).Select(s => s.Precio_Servicio).FirstOrDefault();

                ticket.Subtotal = precio * ticket.Cantidad;

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas", ticket.ID_Venta);
            ViewData["ID_Servicio"] = new SelectList(_context.Servicio, "ID_Servicio", "Descripcion", ticket.ID_Item);
            return View(ticket);
        }

        //Agregar Medicina

        public IActionResult AgregarMedicina()
        {
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas");
            ViewData["ID_Medicina"] = new SelectList(_context.Medicina, "ID_Medicina", "Nombre_Medicina");
            return View();
        }

        // POST: Tickets/Create/Medicina
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarMedicina([Bind("ID_Ticket,ID_Venta,Tipo_item,ID_Item,Cantidad,Subtotal")] Ticket ticket)
        {
            if (ticket.ID_Venta != 0 && ticket.ID_Item != 0)
            {
                ticket.Tipo_item = true;

                var precio = ticket.Tipo_item
                    ? _context.Medicina.Where(m => m.ID_Medicina == ticket.ID_Item).Select(m => m.Precio_Medicina).FirstOrDefault()
                    : _context.Servicio.Where(s => s.ID_Servicio == ticket.ID_Item).Select(s => s.Precio_Servicio).FirstOrDefault();

                ticket.Subtotal = precio * ticket.Cantidad;

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas", ticket.ID_Venta);
            ViewData["ID_Medicina"] = new SelectList(_context.Medicina, "ID_Medicina", "Nombre_Medicina", ticket.ID_Item);
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
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas", ticket.ID_Venta);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Ticket,ID_Venta,Tipo_item,ID_Item,Cantidad,Subtotal")] Ticket ticket)
        {
            if (id != ticket.ID_Ticket)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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
