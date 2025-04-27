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
    public class EstatusCitasController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public EstatusCitasController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: EstatusCitas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstatusCita.ToListAsync());
        }

        // GET: EstatusCitas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estatusCita = await _context.EstatusCita
                .FirstOrDefaultAsync(m => m.ID_Estatus_Cita == id);
            if (estatusCita == null)
            {
                return NotFound();
            }

            return View(estatusCita);
        }

        // GET: EstatusCitas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstatusCitas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Estatus_Cita,Estatus_Cita")] EstatusCita estatusCita)
        {
            if (estatusCita.Estatus_Cita != "")
            {
                _context.Add(estatusCita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estatusCita);
        }

        // GET: EstatusCitas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estatusCita = await _context.EstatusCita.FindAsync(id);
            if (estatusCita == null)
            {
                return NotFound();
            }
            return View(estatusCita);
        }

        // POST: EstatusCitas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Estatus_Cita,Estatus_Cita")] EstatusCita estatusCita)
        {
            if (id != estatusCita.ID_Estatus_Cita)
            {
                return NotFound();
            }

            if (estatusCita.Estatus_Cita != "")
            {
                try
                {
                    _context.Update(estatusCita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstatusCitaExists(estatusCita.ID_Estatus_Cita))
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
            return View(estatusCita);
        }

        // GET: EstatusCitas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estatusCita = await _context.EstatusCita
                .FirstOrDefaultAsync(m => m.ID_Estatus_Cita == id);
            if (estatusCita == null)
            {
                return NotFound();
            }

            return View(estatusCita);
        }

        // POST: EstatusCitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estatusCita = await _context.EstatusCita.FindAsync(id);
            if (estatusCita != null)
            {
                _context.EstatusCita.Remove(estatusCita);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstatusCitaExists(int id)
        {
            return _context.EstatusCita.Any(e => e.ID_Estatus_Cita == id);
        }
    }
}
