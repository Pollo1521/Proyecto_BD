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
    public class ConsultoriosController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public ConsultoriosController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Consultorios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Consultorio.ToListAsync());
        }

        // GET: Consultorios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultorio = await _context.Consultorio
                .FirstOrDefaultAsync(m => m.ID_Consultorio == id);
            if (consultorio == null)
            {
                return NotFound();
            }

            return View(consultorio);
        }

        // GET: Consultorios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consultorios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Consultorio,Piso,Numero_Consultorio")] Consultorio consultorio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consultorio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consultorio);
        }

        // GET: Consultorios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultorio = await _context.Consultorio.FindAsync(id);
            if (consultorio == null)
            {
                return NotFound();
            }
            return View(consultorio);
        }

        // POST: Consultorios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Consultorio,Piso,Numero_Consultorio")] Consultorio consultorio)
        {
            if (id != consultorio.ID_Consultorio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultorio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultorioExists(consultorio.ID_Consultorio))
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
            return View(consultorio);
        }

        // GET: Consultorios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultorio = await _context.Consultorio
                .FirstOrDefaultAsync(m => m.ID_Consultorio == id);
            if (consultorio == null)
            {
                return NotFound();
            }

            return View(consultorio);
        }

        // POST: Consultorios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consultorio = await _context.Consultorio.FindAsync(id);
            if (consultorio != null)
            {
                _context.Consultorio.Remove(consultorio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultorioExists(int id)
        {
            return _context.Consultorio.Any(e => e.ID_Consultorio == id);
        }
    }
}
