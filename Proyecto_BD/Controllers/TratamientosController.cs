using System;
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
    public class TratamientosController : Controller
    {
        private readonly ContextoBaseDatos _context;
        private readonly CorreoElectronico _correoElectronico;

        public TratamientosController(ContextoBaseDatos context, CorreoElectronico correoElectronico)
        {
            _context = context;
            _correoElectronico = correoElectronico;
        }

        // GET: Tratamientos
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Tratamiento.Include(t => t.Receta);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Tratamientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamiento
                .Include(t => t.Receta)
                .FirstOrDefaultAsync(m => m.ID_Tratamiento == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // GET: Tratamientos/Create
        public IActionResult Create()
        {
            TempData.Keep("RecetaActual");
            ViewData["ID_Receta"] = new SelectList(_context.Receta, "ID_Receta", "ID_Receta");
            return View();
        }

        // POST: Tratamientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Tratamiento,ID_Receta,Medicamento,Indicaciones")] Tratamiento tratamiento)
        {
            if (TempData["RecetaActual"] == null)
            {
                return NotFound();
            }
            TempData.Keep("RecetaActual");

            if (TempData["RecetaActual"] is not int idReceta)
            {
                return NotFound();
            }
            TempData.Keep("RecetaActual");

            if (tratamiento.Medicamento != "" && tratamiento.Indicaciones != "")
            {
                TempData.Keep("RecetaActual");
                _context.Add(tratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction("RegistrarTratamiento", "Tratamientos");
            }
            TempData.Keep("RecetaActual");
            ViewData["ID_Receta"] = new SelectList(_context.Receta, "ID_Receta", "ID_Receta", tratamiento.ID_Receta);
            return View(tratamiento);
        }

        // GET: Tratamientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamiento.FindAsync(id);
            if (tratamiento == null)
            {
                return NotFound();
            }
            ViewData["ID_Receta"] = new SelectList(_context.Receta, "ID_Receta", "Diagnostico", tratamiento.ID_Receta);
            return View(tratamiento);
        }

        // POST: Tratamientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Tratamiento,ID_Receta,Medicamento,Indicaciones")] Tratamiento tratamiento)
        {
            if (id != tratamiento.ID_Tratamiento)
            {
                return NotFound();
            }

            if (tratamiento.Medicamento != "" && tratamiento.Indicaciones != "")
            {
                try
                {
                    _context.Update(tratamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TratamientoExists(tratamiento.ID_Tratamiento))
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
            ViewData["ID_Receta"] = new SelectList(_context.Receta, "ID_Receta", "Diagnostico", tratamiento.ID_Receta);
            return View(tratamiento);
        }

        // GET: Tratamientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamiento
                .Include(t => t.Receta)
                .FirstOrDefaultAsync(m => m.ID_Tratamiento == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // POST: Tratamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tratamiento = await _context.Tratamiento.FindAsync(id);
            if (tratamiento != null)
            {
                _context.Tratamiento.Remove(tratamiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TratamientoExists(int id)
        {
            return _context.Tratamiento.Any(e => e.ID_Tratamiento == id);
        }

        [Authorize(Roles = "1, 3")]
        public async Task<IActionResult> RegistrarTratamiento()
        {
            if (TempData["RecetaActual"] is not int idReceta)
            {
                return NotFound();
            }
            TempData.Keep("RecetaActual");

            var tratamientos = await _context.Tratamiento.Include(t => t.Receta).Where(t => t.ID_Receta == idReceta).ToListAsync();

            var view = new List<Tratamiento>();

            foreach(var tratamiento in tratamientos)
            {
                view.Add(new Tratamiento
                {
                    ID_Receta = tratamiento.ID_Receta,
                    Medicamento = tratamiento.Medicamento,
                    Indicaciones = tratamiento.Indicaciones
                });
            }

            return View(view);
        }

        [HttpGet]
        public async Task<IActionResult> GenerarReceta()
        {
            if (TempData["RecetaActual"] is not int idReceta)
            {
                return NotFound();
            }
            TempData.Keep("RecetaActual");

            var receta = await _context.Receta.Include(r => r.Tratamiento).FirstOrDefaultAsync(r => r.ID_Receta == idReceta);

            int idCita = receta.ID_Cita;
            var cita = await _context.Cita.FirstOrDefaultAsync(c => c.ID_Cita == idCita);

            int idPaciente = cita.ID_Paciente;
            var paciente = await _context.Paciente.Include(p => p.Usuario).FirstOrDefaultAsync(p => p.ID_Paciente == idPaciente);

            int idMedico = cita.ID_Medico;
            var medico = await _context.Medico.Include(m => m.Usuario).FirstOrDefaultAsync(m => m.ID_Medico == idMedico);

            var doc = new RecetaPdfDocument(receta, cita, paciente, medico); // tu clase QuestPDF personalizada
            var pdfBytes = doc.GeneratePdf();

            var correo = paciente.Usuario.Correo;
            var mensaje = $"<h2>Receta Médica</h2><p>Diagnóstico: {receta.Diagnostico}</p>";

            try
            {
                await _correoElectronico.EnviarCorreo(correo, "Receta Médica", mensaje, pdfBytes, $"Receta_{receta.ID_Receta}.pdf");
            }
            catch
            {
                return NotFound();
            }

            TempData["RecetaActual"] = null; // Limpiar TempData después de enviar el correo

            var bitacora = new Bitacora
            {
                Fecha_Movimiento = DateTime.Now,
                ID_Medico = idMedico,
                ID_Paciente = idPaciente,
                ID_Cita = idCita
            };

            try
            {
                _context.Bitacoras.Add(bitacora);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return RedirectToAction(controllerName: "Medicos", actionName: "Index");
        }

        public async Task<IActionResult> TratamientosReceta()
        {
            if (TempData["RecetaActual"] is not int idReceta)
            {
                return NotFound();
            }
            TempData.Keep("RecetaActual");

            var contextoBaseDatos = _context.Tratamiento.Include(t => t.Receta).Where(t => t.ID_Receta == idReceta);
            return View(await contextoBaseDatos.ToListAsync());
        }
    }
}
