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
    public class TipoSangresController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public TipoSangresController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: TipoSangres
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoSangre.ToListAsync());
        }

        // GET: TipoSangres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSangre = await _context.TipoSangre
                .FirstOrDefaultAsync(m => m.ID_Tipo_Sangre == id);
            if (tipoSangre == null)
            {
                return NotFound();
            }

            return View(tipoSangre);
        }

        // GET: TipoSangres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoSangres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Tipo_Sangre,Tipo_Sangre")] TipoSangre tipoSangre)
        {
            if (tipoSangre.Tipo_Sangre != "")
            {
                _context.Add(tipoSangre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoSangre);
        }

        // GET: TipoSangres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSangre = await _context.TipoSangre.FindAsync(id);
            if (tipoSangre == null)
            {
                return NotFound();
            }
            return View(tipoSangre);
        }

        // POST: TipoSangres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Tipo_Sangre,Tipo_Sangre")] TipoSangre tipoSangre)
        {
            if (id != tipoSangre.ID_Tipo_Sangre)
            {
                return NotFound();
            }

            if (tipoSangre.Tipo_Sangre != "")
            {
                try
                {
                    _context.Update(tipoSangre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoSangreExists(tipoSangre.ID_Tipo_Sangre))
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
            return View(tipoSangre);
        }

        // GET: TipoSangres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSangre = await _context.TipoSangre
                .FirstOrDefaultAsync(m => m.ID_Tipo_Sangre == id);
            if (tipoSangre == null)
            {
                return NotFound();
            }

            return View(tipoSangre);
        }

        // POST: TipoSangres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoSangre = await _context.TipoSangre.FindAsync(id);
            if (tipoSangre != null)
            {
                _context.TipoSangre.Remove(tipoSangre);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoSangreExists(int id)
        {
            return _context.TipoSangre.Any(e => e.ID_Tipo_Sangre == id);
        }
    }
}
