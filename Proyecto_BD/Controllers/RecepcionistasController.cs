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

namespace Proyecto_BD.Controllers
{
    public class RecepcionistasController : Controller
    {
        private readonly ContextoBaseDatos _context;
        private readonly CorreoElectronico _correoElectronico;

        public RecepcionistasController(ContextoBaseDatos context, CorreoElectronico correoElectronico)
        {
            _context = context;
            _correoElectronico = correoElectronico;
        }

        // GET: Recepcionistas
        [Authorize(Roles = "1, 4")]
        public async Task<IActionResult> Index()
        {

            var contextoBaseDatos = _context.Recepcionista.Include(r => r.Jornada).Include(r => r.Usuario);
            return View(await contextoBaseDatos.ToListAsync());
        }

        // GET: Login Recepcionista
        public async Task<IActionResult> Login()
        {
            int ID_Usuario = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);

            var recepcionista = await _context.Recepcionista
                .Include(r => r.Jornada)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.ID_Usuario == ID_Usuario);

            if (recepcionista == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = recepcionista.ID_Recepcionista });
        }
            

        // GET: Recepcionistas/Details/5
        [Authorize(Roles = "1, 4")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionista
                .Include(r => r.Jornada)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.ID_Recepcionista == id);
            if (recepcionista == null)
            {
                return NotFound();
            }

            return View(recepcionista);
        }

        // GET: Recepcionistas/Create
        [Authorize(Roles = "1, 4")]
        public IActionResult Create(int id)
        {
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "descripcion");
            ViewData["ID_Usuario"] = id;
            return View();
        }

        // POST: Recepcionistas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Recepcionista,ID_Usuario,ID_Jornada")] Recepcionista recepcionista)
        {
            if (recepcionista.ID_Usuario != 0 && recepcionista.ID_Jornada != 0)
            {
                _context.Add(recepcionista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada", recepcionista.ID_Jornada);
            ViewData["ID_Usuario"] = recepcionista.ID_Usuario;
            return View(recepcionista);
        }

        // GET: Recepcionistas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionista.FindAsync(id);
            if (recepcionista == null)
            {
                return NotFound();
            }
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada", recepcionista.ID_Jornada);
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario, "ID_Usuario", "Apellido_Materno", recepcionista.ID_Usuario);
            return View(recepcionista);
        }

        // POST: Recepcionistas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Recepcionista,ID_Usuario,ID_Jornada")] Recepcionista recepcionista)
        {
            if (id != recepcionista.ID_Recepcionista)
            {
                return NotFound();
            }

            if (recepcionista.ID_Usuario != 0 && recepcionista.ID_Jornada != 0)
            {
                try
                {
                    _context.Update(recepcionista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecepcionistaExists(recepcionista.ID_Recepcionista))
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
            ViewData["ID_Jornada"] = new SelectList(_context.Jornada, "ID_Jornada", "ID_Jornada", recepcionista.ID_Jornada);
            ViewData["ID_Usuario"] = new SelectList(_context.Usuario, "ID_Usuario", "Apellido_Materno", recepcionista.ID_Usuario);
            return View(recepcionista);
        }

        // GET: Recepcionistas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionista = await _context.Recepcionista
                .Include(r => r.Jornada)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.ID_Recepcionista == id);
            if (recepcionista == null)
            {
                return NotFound();
            }

            return View(recepcionista);
        }

        // POST: Recepcionistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recepcionista = await _context.Recepcionista.FindAsync(id);
            if (recepcionista != null)
            {
                _context.Recepcionista.Remove(recepcionista);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecepcionistaExists(int id)
        {
            return _context.Recepcionista.Any(e => e.ID_Recepcionista == id);
        }

        // GET: Reecepcionista/ModificarDatosPersonales/5

        [Authorize(Roles = "1, 3, 4")]
        public async Task<IActionResult> EditarDatos()
        {

            //Obtener el ID del usuario desde el token JWT
            int id = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ID_Usuario")?.Value);

            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            ViewData["ID_Tipo_Usuario"] = usuario.ID_Tipo_Usuario;
            return View(usuario);
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarDatos([Bind("ID_Usuario,Nombre,Apellido_Paterno,Apellido_Materno,Correo,CURP,Fecha_Nacimiento,Password,Fecha_Registro,ID_Tipo_Usuario,Estado_Usuario")] Usuario usuario)
        {
            // Verifica si el correo ya existe
            var correoExistente = _context.Usuario.FirstOrDefault(u => u.Correo == usuario.Correo && u.ID_Usuario != usuario.ID_Usuario);

            if (correoExistente != null)
            {
                ViewBag.Error += string.Format("El correo ya esta registrado<br />");
                ViewData["ID_Tipo_Usuario"] = usuario.ID_Tipo_Usuario;
                return View(usuario);
            }

            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                await _correoElectronico.EnviarCorreo(usuario.Correo, "Cambio de Datos", "Se ha realizado un cambio de datos personales");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecepcionistaExists(usuario.ID_Usuario))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IActionResult> EditarJornadas()
        {
            return RedirectToAction(controllerName: "Jornadas", actionName: "Index");
        }

        public async Task<IActionResult> EditarConsultorios()
        {
            return RedirectToAction(controllerName: "Consultorios", actionName: "Index");
        }
    }
}
