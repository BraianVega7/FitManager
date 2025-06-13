using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitManager.Data;
using FitManager.Models;
using FitManager.ViewModels;
using System.Security.Claims;

namespace FitManager.Controllers
{
    public class ActividadController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActividadController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Actividad
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var actividades = await _context.Actividades
                .Where(a => a.UsuarioId == userId)
                .Include(a => a.EntrenadorActividades)
                .ThenInclude(ea => ea.Entrenador)
                .ToListAsync();

            return View(actividades);
        }

        // GET: Actividad/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var actividad = await _context.Actividades
                .Include(a => a.PreciosPorDia)
                .Include(a => a.EntrenadorActividades)
                    .ThenInclude(ea => ea.Entrenador)
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == userId);

            if (actividad == null)
            {
                return NotFound();
            }

            return View(actividad);
        }

        // GET: Actividad/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var entrenadores = _context.Entrenadores
                .Where(e => e.UsuarioId == userId)
                .ToList();

            var viewModel = new CrearActividadViewModel
            {
                Actividad = new Actividad(),
                Precios = Enumerable.Range(1, 5).Select(d => new PrecioPorDiaViewModel
                {
                    DiasPorSemana = d,
                    Precio = 0
                }).ToList(),
                EntrenadoresDisponibles = entrenadores,
                EntrenadorIdsSeleccionados = new List<int>()
            };

            return View(viewModel);
        }

        // POST: Actividad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearActividadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.EntrenadoresDisponibles = _context.Entrenadores
                    .Where(e => e.UsuarioId == userId)
                    .ToList();

                return View(model);
            }

            model.Actividad.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Actividades.Add(model.Actividad);
            await _context.SaveChangesAsync();

            foreach (var precio in model.Precios)
            {
                _context.ActividadPrecios.Add(new ActividadPrecio
                {
                    ActividadId = model.Actividad.Id,
                    DiasPorSemana = precio.DiasPorSemana,
                    Precio = precio.Precio
                });
            }

            if (model.EntrenadorIdsSeleccionados != null && model.EntrenadorIdsSeleccionados.Any())
            {
                foreach (var entrenadorId in model.EntrenadorIdsSeleccionados)
                {
                    _context.EntrenadorActividades.Add(new EntrenadorActividad
                    {
                        ActividadId = model.Actividad.Id,
                        EntrenadorId = entrenadorId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Actividad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var actividad = await _context.Actividades
                .Include(a => a.PreciosPorDia)
                .Include(a => a.EntrenadorActividades)
                .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == userId);

            if (actividad == null)
                return NotFound();

            var entrenadores = await _context.Entrenadores
                .Where(e => e.UsuarioId == userId)
                .ToListAsync();

            var viewModel = new CrearActividadViewModel
            {
                Actividad = actividad,
                Precios = actividad.PreciosPorDia
                    .OrderBy(p => p.DiasPorSemana)
                    .Select(p => new PrecioPorDiaViewModel
                    {
                        DiasPorSemana = p.DiasPorSemana,
                        Precio = p.Precio
                    }).ToList(),
                EntrenadoresDisponibles = entrenadores,
                EntrenadorIdsSeleccionados = actividad.EntrenadorActividades.Select(ea => ea.EntrenadorId).ToList()
            };

            return View(viewModel);
        }

        // POST: Actividad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CrearActividadViewModel model)
        {
            if (id != model.Actividad.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.EntrenadoresDisponibles = await _context.Entrenadores
                    .Where(e => e.UsuarioId == userId)
                    .ToListAsync();
                return View(model);
            }

            var userIdCheck = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var actividadEnDb = await _context.Actividades
                .Include(a => a.PreciosPorDia)
                .Include(a => a.EntrenadorActividades)
                .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == userIdCheck);

            if (actividadEnDb == null)
                return NotFound();

            actividadEnDb.Nombre = model.Actividad.Nombre;

            foreach (var precioVm in model.Precios)
            {
                var precioEnDb = actividadEnDb.PreciosPorDia
                    .FirstOrDefault(p => p.DiasPorSemana == precioVm.DiasPorSemana);

                if (precioEnDb != null)
                {
                    precioEnDb.Precio = precioVm.Precio;
                }
            }

            actividadEnDb.EntrenadorActividades.Clear();
            if (model.EntrenadorIdsSeleccionados != null && model.EntrenadorIdsSeleccionados.Any())
            {
                foreach (var entrenadorId in model.EntrenadorIdsSeleccionados)
                {
                    actividadEnDb.EntrenadorActividades.Add(new EntrenadorActividad
                    {
                        EntrenadorId = entrenadorId,
                        ActividadId = actividadEnDb.Id
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Actividad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var actividad = await _context.Actividades
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == userId);

            if (actividad == null)
            {
                return NotFound();
            }

            return View(actividad);
        }

        // POST: Actividad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var actividad = await _context.Actividades
                .Include(a => a.PreciosPorDia)
                .Include(a => a.EntrenadorActividades)
                .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == userId);

            if (actividad != null)
            {
                _context.ActividadPrecios.RemoveRange(actividad.PreciosPorDia);
                _context.EntrenadorActividades.RemoveRange(actividad.EntrenadorActividades);
                _context.Actividades.Remove(actividad);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ActividadExists(int id)
        {
            return _context.Actividades.Any(e => e.Id == id);
        }
    }
}
