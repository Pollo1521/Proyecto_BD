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
    public class CitasController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public CitasController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Cita.Include(c => c.EstatusCita).Include(c => c.Medico).Include(c => c.Paciente);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .Include(c => c.EstatusCita)
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.ID_Cita == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            var pacientes = _context.Paciente.Include(x => x.Usuario).Where(x => x.Usuario != null).ToList();
            var medicos = _context.Medico.Include(x => x.Usuario).Where(x => x.Usuario != null).ToList();

            ViewData["ID_Estatus_Cita"] = new SelectList(_context.EstatusCita, "ID_Estatus_Cita", "Estatus_Cita");
            ViewData["ID_Medico"] = new SelectList(medicos, "ID_Medico", "Usuario.Nombre");
            ViewData["ID_Paciente"] = new SelectList(pacientes, "ID_Paciente", "Usuario.Nombre");
            return View();
        }

        // POST: Citas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Cita,ID_Paciente,ID_Medico,Fecha_Registro,Fecha_Cita,Hora_Cita,ID_Estatus_Cita")] Cita cita)
        {
            if (cita.ID_Medico != 0)  //FALTA VALIDAR CITAS
            {
                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var pacientes = _context.Paciente.Include(x => x.Usuario).Where(x => x.Usuario != null).ToList();
            var medicos = _context.Medico.Include(x => x.Usuario).Where(x => x.Usuario != null).ToList();

            ViewData["ID_Estatus_Cita"] = new SelectList(_context.EstatusCita, "ID_Estatus_Cita", "Estatus_Cita", cita.ID_Estatus_Cita);
            ViewData["ID_Medico"] = new SelectList(medicos, "ID_Medico", "Usuario.Nombre", cita.ID_Medico);
            ViewData["ID_Paciente"] = new SelectList(pacientes, "ID_Paciente", "Usuario.Nombre", cita.ID_Paciente);
            return View(cita);
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }

            var pacientes = _context.Paciente.Include(x => x.Usuario).Where(x => x.Usuario != null).ToList();
            var medicos = _context.Medico.Include(x => x.Usuario).Where(x => x.Usuario != null).ToList();

            ViewData["ID_Estatus_Cita"] = new SelectList(_context.EstatusCita, "ID_Estatus_Cita", "Estatus_Cita", cita.ID_Estatus_Cita);
            ViewData["ID_Medico"] = new SelectList(medicos, "ID_Medico", "Usuario.Nombre", cita.ID_Medico);
            ViewData["ID_Paciente"] = new SelectList(pacientes, "ID_Paciente", "Usuario.Nombre", cita.ID_Paciente);
            return View(cita);
        }

        // POST: Citas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Cita,ID_Paciente,ID_Medico,Fecha_Registro,Fecha_Cita,Hora_Cita,ID_Estatus_Cita")] Cita cita)
        {
            if (id != cita.ID_Cita)
            {
                return NotFound();
            }

            if (cita.ID_Medico != 0)  //FALTA VALIDAR CITAS
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.ID_Cita))
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

            var pacientes = _context.Paciente.Include(x => x.Usuario).Where(x => x.Usuario != null).ToList();
            var medicos = _context.Medico.Include(x => x.Usuario).Where(x => x.Usuario != null).ToList();

            ViewData["ID_Estatus_Cita"] = new SelectList(_context.EstatusCita, "ID_Estatus_Cita", "Estatus_Cita", cita.ID_Estatus_Cita);
            ViewData["ID_Medico"] = new SelectList(medicos, "ID_Medico", "Usuario.Nombre", cita.ID_Medico);
            ViewData["ID_Paciente"] = new SelectList(pacientes, "ID_Paciente", "Usuario.Nombre", cita.ID_Paciente);
            return View(cita);
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .Include(c => c.EstatusCita)
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.ID_Cita == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cita = await _context.Cita.FindAsync(id);
            if (cita != null)
            {
                _context.Cita.Remove(cita);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
            return _context.Cita.Any(e => e.ID_Cita == id);
        }
    }
}
