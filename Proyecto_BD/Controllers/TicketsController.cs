﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_BD.Models;
using Proyecto_BD.Utilities;
using QuestPDF.Fluent;

namespace Proyecto_BD.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ContextoBaseDatos _context;
        private readonly CorreoElectronico _correoElectronico;

        public TicketsController(ContextoBaseDatos context, CorreoElectronico correoElectronico)
        {
            _context = context;
            _correoElectronico = correoElectronico;
        }

        // GET: Tickets
        [Authorize(Roles = "1, 4")]
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
        [Authorize(Roles = "1, 4")]
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
        [Authorize(Roles = "1, 4")]
        public IActionResult AgregarServicio()
        {
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas");
            ViewData["ID_Servicio"] = new SelectList(_context.Servicio, "ID_Servicio", "Descripcion");
            TempData.Keep("VentaActual");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarServicio([Bind("ID_Ticket,ID_Venta,Tipo_item,ID_Item,Cantidad,Subtotal")] Ticket ticket)
        {
            if (TempData["VentaActual"] == null)
            {
                return NotFound();
            }

            TempData.Keep("VentaActual");

            if (TempData["VentaActual"] is int idVenta)
            {
                TempData.Keep("VentaActual");
            }
            else
            {
                return NotFound();
            }


            ticket.ID_Venta = idVenta;

            if (ticket.ID_Venta != 0 && ticket.ID_Item != 0)
            {
                ticket.Tipo_item = false;

                var precio = ticket.Tipo_item
                    ? _context.Medicina.Where(m => m.ID_Medicina == ticket.ID_Item).Select(m => m.Precio_Medicina).FirstOrDefault()
                    : _context.Servicio.Where(s => s.ID_Servicio == ticket.ID_Item).Select(s => s.Precio_Servicio).FirstOrDefault();

                ticket.Subtotal = precio * ticket.Cantidad;

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction("RegistrarVenta", "Tickets");
            }
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas", ticket.ID_Venta);
            ViewData["ID_Servicio"] = new SelectList(_context.Servicio, "ID_Servicio", "Descripcion", ticket.ID_Item);
            return View(ticket);
        }

        //Agregar Medicina
        [Authorize(Roles = "1, 4")]
        public IActionResult AgregarMedicina()
        {
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas");
            ViewData["ID_Medicina"] = new SelectList(_context.Medicina, "ID_Medicina", "Nombre_Medicina");
            TempData.Keep("VentaActual");
            return View();
        }

        // POST: Tickets/Create/Medicina
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarMedicina([Bind("ID_Ticket,ID_Venta,Tipo_item,ID_Item,Cantidad,Subtotal")] Ticket ticket)
        {
            if (TempData["VentaActual"] == null)
            {
                return NotFound();
            }

            TempData.Keep("VentaActual");

            if (TempData["VentaActual"] is int idVenta)
            {
                TempData.Keep("VentaActual");
            }
            else
            {
                return NotFound();
            }

            ticket.ID_Venta = idVenta;
            ticket.Tipo_item = true;

            if (ticket.ID_Venta != 0)
            {
                var medicina = await _context.Medicina.FindAsync(ticket.ID_Item);

                if(medicina.Cantidad < ticket.Cantidad)
                {
                    TempData["Error"] = $"Solo hay {medicina.Cantidad} unidades";
                    ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas", ticket.ID_Venta);
                    ViewData["ID_Medicina"] = new SelectList(_context.Medicina, "ID_Medicina", "Nombre_Medicina", ticket.ID_Item);
                    return View(ticket);
                }

                medicina.Cantidad -= ticket.Cantidad;

                var precio = ticket.Tipo_item
                    ? _context.Medicina.Where(m => m.ID_Medicina == ticket.ID_Item).Select(m => m.Precio_Medicina).FirstOrDefault()
                    : _context.Servicio.Where(s => s.ID_Servicio == ticket.ID_Item).Select(s => s.Precio_Servicio).FirstOrDefault();

                ticket.Subtotal = precio * ticket.Cantidad;

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction("RegistrarVenta", "Tickets");
            }
            ViewData["ID_Venta"] = new SelectList(_context.Venta, "ID_Ventas", "ID_Ventas", ticket.ID_Venta);
            ViewData["ID_Medicina"] = new SelectList(_context.Medicina, "ID_Medicina", "Nombre_Medicina", ticket.ID_Item);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "1, 4")]
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
        [Authorize(Roles = "1, 4")]
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

        [Authorize(Roles = "1, 4")]
        public async Task<IActionResult> RegistrarVenta()
        {

            if (TempData["VentaActual"] is not int idVenta)
            {
                return NotFound();
            }

            TempData.Keep("VentaActual");

            var tickets = await _context.Ticket.Include(t => t.Venta).Where(t => t.ID_Venta == idVenta).ToListAsync();

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

        [Authorize(Roles = "1, 4")]
        public IActionResult GenerarTicket()
        {
            if (TempData["VentaActual"] is not int idVenta)
            {
                return NotFound();
            }
            TempData.Keep("VentaActual");
            // Aquí podrías redirigir a la acción GenerarTicket para generar el PDF
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerarTicket(EnviarTicketViewModel model)
        {
            if (TempData["VentaActual"] is not int idVenta)
            {
                return NotFound();
            }
            TempData.Keep("VentaActual");

            var tickets = await _context.Ticket.Include(t => t.Venta).Where(t => t.ID_Venta == idVenta).ToListAsync();

            foreach(var t in tickets)
            {
                t.Medicina = t.Tipo_item ? await _context.Medicina.FindAsync(t.ID_Item) : null;
                t.Servicio = !t.Tipo_item ? await _context.Servicio.FindAsync(t.ID_Item) : null;
            }

            var venta = tickets.FirstOrDefault()?.Venta;
            var doc = new TicketDocument(tickets, venta);

            var pdfBytes = doc.GeneratePdf();

            string fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            var mensaje = string.Format("<h1>Hola</h1> <br /> <p>Se completo su compra de {0}</p>", fecha);

            try
            {
                await _correoElectronico.EnviarCorreo(model.CorreoElectronico, "Ticket de Venta", mensaje, pdfBytes, $"Ticket_Venta_{idVenta}.pdf");
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return RedirectToAction(controllerName: "Ventas", actionName: "Index");
        }
    }
}
