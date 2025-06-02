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
    public class MedicinasController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public MedicinasController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Medicinas
        [Authorize(Roles = "1, 4")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Medicina.ToListAsync());
        }

        // GET: Medicinas/Details/5
        [Authorize(Roles = "1, 4")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicina = await _context.Medicina
                .FirstOrDefaultAsync(m => m.ID_Medicina == id);
            if (medicina == null)
            {
                return NotFound();
            }

            return View(medicina);
        }

        // GET: Medicinas/Create
        [Authorize(Roles = "1, 4")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Medicina,Cantidad,Precio_Medicina,Nombre_Medicina")] Medicina medicina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicina);
        }

        // GET: Medicinas/Edit/5
        [Authorize(Roles = "1, 4")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicina = await _context.Medicina.FindAsync(id);
            if (medicina == null)
            {
                return NotFound();
            }
            return View(medicina);
        }

        // POST: Medicinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Medicina,Cantidad,Precio_Medicina,Nombre_Medicina")] Medicina medicina)
        {
            if (id != medicina.ID_Medicina)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicinaExists(medicina.ID_Medicina))
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
            return View(medicina);
        }

        // GET: Medicinas/Delete/5
        [Authorize(Roles = "1, 4")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicina = await _context.Medicina
                .FirstOrDefaultAsync(m => m.ID_Medicina == id);
            if (medicina == null)
            {
                return NotFound();
            }

            return View(medicina);
        }

        // POST: Medicinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicina = await _context.Medicina.FindAsync(id);
            if (medicina != null)
            {
                _context.Medicina.Remove(medicina);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicinaExists(int id)
        {
            return _context.Medicina.Any(e => e.ID_Medicina == id);
        }
    }
}
