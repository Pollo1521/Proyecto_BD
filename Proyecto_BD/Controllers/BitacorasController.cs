using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Proyecto_BD.Models;
using Proyecto_BD.Utilities;

namespace Proyecto_BD.Controllers
{
    public class BitacorasController : Controller
    {
        private readonly ContextoBaseDatos _context;

        public BitacorasController(ContextoBaseDatos context)
        {
            _context = context;
        }

        // GET: Bitacoras
        [Authorize(Roles = "1, 3")]
        public async Task<IActionResult> Index()
        {
            if (TempData["PacienteActual"] is not int idPaciente)
            {
                return NotFound();
            }
            TempData.Keep("PacienteActual");

            var contextoBaseDatos = _context.Bitacoras
                                    .Include(b => b.Cita).ThenInclude(c => c.Recetas)
                                    .Include(b => b.Paciente).ThenInclude(p => p.Usuario)
                                    .Include(b => b.Medico).ThenInclude(m => m.Usuario)
                                    .Include(b => b.Medico).ThenInclude(m => m.Especialidad)
                                    .Include(b => b.Medico).ThenInclude(m => m.Consultorio)
                                    .Where(b => b.ID_Paciente == idPaciente);


            return View(await contextoBaseDatos.ToListAsync());
        }
    }
}
