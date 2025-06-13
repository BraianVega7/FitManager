using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitManager.Data;
using FitManager.Models;
using System.Security.Claims;
using FitManager.ViewModels;

namespace FitManager.Controllers
{
    public class EntrenadorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntrenadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Entrenador
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var entrenadores = await _context.Entrenadores
                .Where(e => e.UsuarioId == userId)
                .Include(e => e.EntrenadorActividades)
                .ThenInclude(ea => ea.Actividad)
                .ToListAsync();
            return View(entrenadores);
        }

        // GET: Entrenador/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var entrenador = await _context.Entrenadores
                .Include(e => e.EntrenadorActividades)
                .ThenInclude(ea => ea.Actividad)
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == userId);
            if (entrenador == null)
            {
                return NotFound();
            }

            return View(entrenador);
        }

        // GET: Entrenador/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var actividades = _context.Actividades
                .Where(a => a.UsuarioId == userId)
                .ToList();

            var model = new CrearEntrenadorViewModel
            {
                Entrenador = new Entrenador { UsuarioId = userId },
                ActividadDisponible = actividades,
                ActividadIdSeleccionadas = new List<int>()
            };
            return View(model);
        }

        // POST: Entrenador/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearEntrenadorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.ActividadDisponible = await _context.Actividades
                    .Where(a => a.UsuarioId == userId)
                    .ToListAsync();
                return View(model);
            }

            var userIdFinal = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var entrenadorNuevo = new Entrenador
            {
                Nombre = model.Entrenador.Nombre,
                Apellido = model.Entrenador.Apellido,
                Email = model.Entrenador.Email,
                SueldoMensual = model.Entrenador.SueldoMensual,
                Telefono = model.Entrenador.Telefono,
                UsuarioId = userIdFinal
            };

            _context.Entrenadores.Add(entrenadorNuevo);
            await _context.SaveChangesAsync();

            foreach (var actividadId in model.ActividadIdSeleccionadas)
            {
                _context.EntrenadorActividades.Add(new EntrenadorActividad
                {
                    EntrenadorId = entrenadorNuevo.Id,
                    ActividadId = actividadId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Entrenador/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var entrenador = await _context.Entrenadores
                .Include(e => e.EntrenadorActividades)
                .ThenInclude(ea => ea.Actividad)
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == userId);
            if (entrenador == null)
            {
                return NotFound();
            }

            var actividades = await _context.Actividades
                .Where(a => a.UsuarioId == userId)
                .ToListAsync();
            
            var ViewModel = new CrearEntrenadorViewModel
            {
                Entrenador = entrenador,
                ActividadDisponible = actividades,
                ActividadIdSeleccionadas = entrenador.EntrenadorActividades.Select(ea => ea.ActividadId).ToList()
            };
            return View(ViewModel);
        }

        // POST: Entrenador/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CrearEntrenadorViewModel model)
        {
            if (id != model.Entrenador.Id)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid)
            {
                model.ActividadDisponible = await _context.Actividades
                    .Where(a => a.UsuarioId == userId)
                    .ToListAsync();
                return View(model);
            }

            var entrenadorEnDb = await _context.Entrenadores
               .Include(e => e.EntrenadorActividades)
               .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == userId);

            if (entrenadorEnDb == null)
                return NotFound();

            entrenadorEnDb.Nombre = model.Entrenador.Nombre;
            entrenadorEnDb.Apellido = model.Entrenador.Apellido;
            entrenadorEnDb.Email = model.Entrenador.Email;
            entrenadorEnDb.SueldoMensual = model.Entrenador.SueldoMensual;
            entrenadorEnDb.Telefono = model.Entrenador.Telefono;

            entrenadorEnDb.EntrenadorActividades.Clear();
            foreach (var actividadId in model.ActividadIdSeleccionadas)
            {
                entrenadorEnDb.EntrenadorActividades.Add(new EntrenadorActividad
                {
                    ActividadId = actividadId,
                    EntrenadorId = entrenadorEnDb.Id
                });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Entrenador/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var entrenador = await _context.Entrenadores
                .Include(e => e.EntrenadorActividades)
                .ThenInclude(ea => ea.Actividad)
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == userId);

            if (entrenador == null)
            {
                return NotFound();
            }

            return View(entrenador);
        }

        // POST: Entrenador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var entrenador = await _context.Entrenadores
                .Include(e => e.EntrenadorActividades)
                .ThenInclude(ea => ea.Actividad)
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == userId);
            if (entrenador != null)
            {
                _context.Entrenadores.Remove(entrenador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntrenadorExists(int id)
        {
            return _context.Entrenadores.Any(e => e.Id == id);
        }
    }
}
