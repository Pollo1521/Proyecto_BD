using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AspNetCoreGeneratedDocument;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_BD.Models;
using Proyecto_BD.Utilities;
using QuestPDF.Fluent;


namespace Proyecto_BD.Controllers
{
    public class CitasController : Controller
    {
        private readonly ContextoBaseDatos _context;
        private readonly CorreoElectronico _correoElectronico;

        public CitasController(ContextoBaseDatos context, CorreoElectronico correoElectronico)
        {
            _context = context;
            _correoElectronico = correoElectronico;
        }

        // GET: Citas
        [Authorize(Roles = "1, 2, 3")]
        public async Task<IActionResult> Index()
        {
            var contextoBaseDatos = _context.Cita.Include(c => c.CitasHorario).Include(c => c.EstatusCita).Include(c => c.Medico).ThenInclude(m => m.Usuario).Include(c => c.Paciente).ThenInclude(p => p.Usuario).OrderByDescending(c => c.Fecha_Cita);
            
            if (User.IsInRole("2"))
            {
                // Si el usuario es paciente, filtra las citas por el ID del paciente
                int ID_Usuario = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);
                var paciente = await _context.Paciente.FirstOrDefaultAsync(p => p.ID_Usuario == ID_Usuario);
                if (paciente == null)
                {
                    return NotFound();
                }
                TempData["TipoUsuario"] = "Paciente";
                var citasPaciente = contextoBaseDatos.Where(c => c.ID_Paciente == paciente.ID_Paciente);
                return View(await citasPaciente.ToListAsync());
            }

            // Si el usuario es médico, filtra las citas por el ID del médico
            if (User.IsInRole("3"))
            {
                TempData["TipoUsuario"] = "Doctor";
                int ID_Usuario = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);
                var medico = await _context.Medico.FirstOrDefaultAsync(m => m.ID_Usuario == ID_Usuario);
                if (medico == null)
                {
                    return NotFound();
                }
                var citasMedico = contextoBaseDatos.Where(c => c.ID_Medico == medico.ID_Medico && (c.ID_Estatus_Cita == 2 || c.ID_Estatus_Cita == 3)).OrderBy(c => c.Fecha_Cita);
                return View(await citasMedico.ToListAsync());
            }

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
                .Include(c => c.CitasHorario)
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
            int ID_Usuario = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);

            var paciente = _context.Paciente.FirstOrDefault(p => p.ID_Usuario == ID_Usuario);
            if (paciente == null)
            {
                return NotFound();
            }

            ViewData["ID_Paciente"] = paciente.ID_Paciente;
            ViewData["Especialidades"] = new SelectList(_context.Especialidad, "ID_Especialidad", "Descripcion");
            return View(new Cita { ID_Paciente = paciente.ID_Paciente });
        }

        // POST: Citas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Cita,ID_Paciente,ID_Medico,Fecha_Registro,Fecha_Cita,ID_Cita_Horario,ID_Estatus_Cita")] Cita cita)
        {
            cita.Fecha_Registro = DateTime.Now;
            cita.ID_Estatus_Cita = 1;

            int ID_Usuario = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);
            var paciente = _context.Paciente.FirstOrDefault(p => p.ID_Usuario == ID_Usuario);

            if (paciente == null)
            {
                return NotFound();
            }

            if (cita.Fecha_Cita < DateTime.Now.Date.AddDays(2) || cita.Fecha_Cita > DateTime.Now.Date.AddMonths(3))
            {
                ModelState.AddModelError("Fecha_Cita", "La fecha de la cita debe ser al menos un día después de hoy y no más de 3 meses en el futuro.");
                return View(new Cita());
            }

            bool citaOcupada = _context.Cita
                .Any(c => c.ID_Medico == cita.ID_Medico &&
                          c.Fecha_Cita.Date == cita.Fecha_Cita.Date &&
                          c.ID_Cita_Horario == cita.ID_Cita_Horario
                );

            bool citaPendiente = _context.Cita
                                .Any(c => c.ID_Medico == cita.ID_Medico &&
                                c.ID_Paciente == paciente.ID_Paciente &&
                                (c.ID_Estatus_Cita == 1 || c.ID_Estatus_Cita == 2));

            var horario = _context.CitasHorario
                .FirstOrDefault(h => h.ID_Horario == cita.ID_Cita_Horario);

            var consultorio = _context.Medico.Include(m => m.Consultorio)
                .FirstOrDefault(m => m.ID_Medico == cita.ID_Medico);

            if (citaOcupada)
            {
                ModelState.AddModelError(string.Empty, "El horario seleccionado ya está ocupado para el medico seleccionado");
                ViewData["ID_Paciente"] = paciente.ID_Paciente;
                ViewData["Especialidades"] = new SelectList(_context.Especialidad, "ID_Especialidad", "Descripcion");
                return View(new Cita());
            }

            if (citaPendiente)
            {
                ModelState.AddModelError(string.Empty, "Ya se tiene una cita agendada con dicho doctor");
                ViewData["ID_Paciente"] = paciente.ID_Paciente;
                ViewData["Especialidades"] = new SelectList(_context.Especialidad, "ID_Especialidad", "Descripcion");
                return View(new Cita());
            }

            if (cita.ID_Medico != 0 || cita.ID_Cita_Horario != 0)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    _context.Cita.Add(cita);
                    await _context.SaveChangesAsync();

                    var pago = new Pago
                    {
                        ID_Cita = cita.ID_Cita,
                        Estado_Pago = false,
                        ComprobantePago = ""
                    };

                    _context.Pago.Add(pago);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                //Obtener la información necesaria para el PDF
                var infoPDF = await _context.Cita
                    .Include(c => c.Paciente)
                        .ThenInclude(p => p.Usuario)
                    .Include(c => c.Medico)
                        .ThenInclude(m => m.Usuario)
                    .Include(c => c.Medico.Especialidad)
                    .Include(c => c.Medico.Consultorio)
                    .Include(c => c.CitasHorario)
                    .FirstOrDefaultAsync(c => c.ID_Cita == cita.ID_Cita);

                //Crear objeto para el PDF
                var datosPago = new LineaPagoData
                {
                    Paciente = $"{cita.Paciente.Usuario.Nombre} {cita.Paciente.Usuario.Apellido_Paterno} {cita.Paciente.Usuario.Apellido_Materno}",
                    CURP = cita.Paciente.Usuario.CURP,
                    FechaCita = cita.Fecha_Cita,
                    HoraCita = cita.CitasHorario.Hora_Cita.ToString(@"hh\:mm tt"),
                    Medico = $"{cita.Medico.Usuario.Nombre} {cita.Medico.Usuario.Apellido_Paterno}",
                    Especialidad = cita.Medico.Especialidad.Descripcion,
                    Consultorio = $"Piso {cita.Medico.Consultorio.Piso}, No. {cita.Medico.Consultorio.Numero_Consultorio}",
                    Precio = cita.Medico.Especialidad.PrecioCita,
                    LinkPago = $"{cita.ID_Cita}"
                };

                //Generar PDF
                var documento = new LineaPagoDocument(datosPago);
                var pdfBytes = documento.GeneratePdf();

                // Enviar correo de confirmación
                var idUsuario = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);
                var usuario = _context.Usuario.FirstOrDefault(u => u.ID_Usuario == idUsuario);
                var mensaje = string.Format("<h1>Confirmacion de Cita</h1> <br /> <p>Se registro correctamente la cita para el: {0} a las {1}. Consultorio: {2}</p>",
                                            cita.Fecha_Cita.ToString("d"), horario.Hora_Cita.ToString("hh\\:mm"), consultorio.Consultorio.Numero_Consultorio);
                try
                {
                    await _correoElectronico.EnviarCorreo(usuario.Correo, "Linea de Pago", mensaje, pdfBytes, $"Linea_Pago_{cita.ID_Cita}.pdf");
                }
                catch (Exception ex)
                {
                    //Holi, no se que poner aqui XD
                }

                BackgroundJob.Schedule<CancelarCita>(x => x.CancelarCitaNoPagada(cita.ID_Cita), TimeSpan.FromHours(8));

                return RedirectToAction(controllerName: "Pacientes", actionName: "Index");
            }

            ViewData["ID_Paciente"] = paciente.ID_Paciente;
            ViewData["Especialidades"] = new SelectList(_context.Especialidad, "ID_Especialidad", "Descripcion");

            return View(new Cita());
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
            ViewData["ID_Cita_Horario"] = new SelectList(_context.CitasHorario, "ID_Horario", "ID_Horario", cita.ID_Cita_Horario);
            ViewData["ID_Estatus_Cita"] = new SelectList(_context.EstatusCita, "ID_Estatus_Cita", "Estatus_Cita", cita.ID_Estatus_Cita);
            ViewData["ID_Medico"] = new SelectList(_context.Medico, "ID_Medico", "Cedula", cita.ID_Medico);
            ViewData["ID_Paciente"] = new SelectList(_context.Paciente, "ID_Paciente", "Alergia", cita.ID_Paciente);
            return View(cita);
        }

        // POST: Citas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Cita,ID_Paciente,ID_Medico,Fecha_Registro,Fecha_Cita,ID_Cita_Horario,ID_Estatus_Cita")] Cita cita)
        {
            if (id != cita.ID_Cita)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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
            ViewData["ID_Cita_Horario"] = new SelectList(_context.CitasHorario, "ID_Horario", "ID_Horario", cita.ID_Cita_Horario);
            ViewData["ID_Estatus_Cita"] = new SelectList(_context.EstatusCita, "ID_Estatus_Cita", "Estatus_Cita", cita.ID_Estatus_Cita);
            ViewData["ID_Medico"] = new SelectList(_context.Medico, "ID_Medico", "Cedula", cita.ID_Medico);
            ViewData["ID_Paciente"] = new SelectList(_context.Paciente, "ID_Paciente", "Alergia", cita.ID_Paciente);
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
                .Include(c => c.CitasHorario)
                .Include(c => c.EstatusCita)
                .Include(c => c.Medico).ThenInclude(m => m.Usuario)
                .Include(c => c.Paciente).ThenInclude(p => p.Usuario)
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
            if (cita == null)
            {
                return NotFound();                
            }

            cita.ID_Estatus_Cita = 5; // Cambiar el estatus a cancelada
            _context.Cita.Update(cita);
            await _context.SaveChangesAsync();

            // Enviar correo de cancelación
            var paciente = await _context.Paciente
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.ID_Paciente == cita.ID_Paciente);

            if (paciente == null)
            {
                return NotFound();
            }

            var diferencia = cita.Fecha_Cita - DateTime.Now;
            string mensaje;

            if (diferencia.TotalHours >= 48)
            {
                // Reembolso completo
                mensaje = string.Format("<h1>Cancelacion de Cita</h1> <br /> <p>Se cancelo su cita del dia: {0} Reembolso recibido: 100%</p>", cita.Fecha_Cita);
            }
            else if (diferencia.TotalHours >= 24)
            {
                // Reembolso del 50%
                mensaje = string.Format("<h1>Cancelacion de Cita</h1> <br /> <p>Se cancelo su cita del dia: {0} Reembolso recibido: 50%</p>", cita.Fecha_Cita);
            }
            else
            {
                // Sin reembolso
                mensaje = string.Format("<h1>Cancelacion de Cita</h1> <br /> <p>Se cancelo su cita del dia: {0} Reembolso recibido: 0%</p>", cita.Fecha_Cita);
            }

            try
            {
                await _correoElectronico.EnviarCorreo(paciente.Usuario.Correo, "Cancelacion Cita", mensaje);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //Holi, no se que poner aqui XD
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
            return _context.Cita.Any(e => e.ID_Cita == id);
        }

        //FUNCIONES AJAX

        [HttpGet]
        public JsonResult GetMedicosPorEspecialidad(int idEspecialidad)
        {
            var medicos = _context.Medico
                .Where(m => m.ID_Especialidad == idEspecialidad)
                .Select(m => new SelectListItem
                {
                    Value = m.ID_Medico.ToString(),
                    Text = _context.Usuario
                        .Where(u => u.ID_Usuario == m.ID_Usuario)
                        .Select(u => u.Nombre + " " + u.Apellido_Paterno + " " + u.Apellido_Materno)
                        .FirstOrDefault()
                })
                .ToList();
            return Json(medicos);
        }

        [HttpGet]
        public JsonResult GetHorariosDisponibles(int idMedico, DateTime fecha)
        {
            //Obtener Jornada
            var medico = _context.Medico
                .Include(m => m.Jornada)
                .FirstOrDefault(m => m.ID_Medico == idMedico);

            if (medico == null)
            {
                return Json(new List<SelectListItem>()); // Retorna una lista vacía si no se encuentra el médico
            }

            bool esMatutino = medico.Jornada.ID_Jornada == 1;

            var horariosDisponibles = _context.CitasHorario
                .Where(h => h.JornadaHorario == esMatutino)
                .ToList();

            var horariosOcupados = _context.Cita
                .Where(c => c.ID_Medico == idMedico &&
                            c.Fecha_Cita.Date == fecha.Date &&
                            c.ID_Estatus_Cita != 5) // Solo citas confirmadas
                .Select(c => c.ID_Cita_Horario)
                .ToList();

            var horarios = horariosDisponibles
                .Where(h => !horariosOcupados.Contains(h.ID_Horario))
                .Select(h => new SelectListItem
                {
                    Value = h.ID_Horario.ToString(),
                    Text = h.Hora_Cita.ToString("hh\\:mm tt")
                })
                .ToList();

            return Json(horarios);
        }

        public async Task<IActionResult> Cancelar()
        {
            return RedirectToAction(controllerName: "Pacientes", actionName: "Index");
        }

        public async Task<IActionResult> CancelarDoctor()
        {
            return RedirectToAction(controllerName: "Medicos", actionName: "Index");
        }

        public async Task<IActionResult> AgregarReceta(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TempData["CitaActual"] = id;
            TempData.Keep("CitaActual");
            return RedirectToAction(controllerName: "Recetas", actionName: "Create");
        }

        public async Task<IActionResult> VerBitacora(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TempData["PacienteActual"] = id;
            TempData.Keep("PacienteActual");
            return RedirectToAction(controllerName: "Bitacoras", actionName: "Index");
        }
    }
}
