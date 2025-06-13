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
using System.Diagnostics;

namespace FitManager.Controllers
{
    public class SocioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SocioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Socio
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var socios = await _context.Socios
                .Where(s => s.UsuarioId == userId)
                .Include(s => s.SocioActividades)
                .ThenInclude(sa => sa.Actividad)
                .ToListAsync();
            return View(socios);
        }

        // GET: Socio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var socio = await _context.Socios
                .Include(s => s.SocioActividades)
                .ThenInclude(sa => sa.Actividad)
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == userId);
            if (socio == null)
            {
                return NotFound();
            }

            return View(socio);
        }

        // GET: Socio/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var actividades = _context.Actividades
                .Where(a => a.UsuarioId == userId)
                .ToList();

            var model= new CrearSocioViewModel
            {
                Actividades = actividades.Select(a => new SocioActividadViewModel
                {
                    ActividadId = a.Id,
                    NombreActividad = a.Nombre,
                    Seleccionado = false,
                    DiasPorSemana = 1
                }).ToList()
            };

            return View(model);
        }

        // POST: Socio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearSocioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var actividades = _context.Actividades.ToList();
                model.Actividades = actividades.Select(a => new SocioActividadViewModel
                {
                    ActividadId = a.Id,
                    NombreActividad = a.Nombre,
                    Seleccionado = model.Actividades.FirstOrDefault(vm => vm.ActividadId == a.Id)?.Seleccionado ?? false,
                    DiasPorSemana = model.Actividades.FirstOrDefault(vm => vm.ActividadId == a.Id)?.DiasPorSemana ?? 1
                }).ToList();

                return View(model);
            };

            var nuevoSocio = new Socio
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Dni = model.Dni,
                Telefono = model.Telefono,
                Email = model.Email,
                FechaIngreso = model.FechaIngreso,
                Domicilio = model.Domicilio,
                Localidad = model.Localidad,
                UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            foreach(var actividad in model.Actividades.Where(a => a.Seleccionado))
            {
                nuevoSocio.SocioActividades.Add(new SocioActividad
                {
                    ActividadId = actividad.ActividadId,
                    DiasPorSemana = actividad.DiasPorSemana,
                });
            }

            _context.Socios.Add(nuevoSocio);
            await _context.SaveChangesAsync();
  
            return RedirectToAction(nameof(Index));
        }

        // GET: Socio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Socios == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var socio = await _context.Socios
                .Include(s => s.SocioActividades)
                .ThenInclude(sa => sa.Actividad)
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == userId);

            if (socio == null)
            {
                return NotFound();
            }

            var todasLasActividades = await _context.Actividades.ToListAsync();
            var actividadesVM = todasLasActividades.Select(a => new SocioActividadViewModel
            {
                ActividadId = a.Id,
                NombreActividad = a.Nombre,
                Seleccionado = socio.SocioActividades.Any(sa => sa.ActividadId == a.Id),
                DiasPorSemana = socio.SocioActividades.FirstOrDefault(sa => sa.ActividadId == a.Id)?.DiasPorSemana ?? 1
            }).ToList();

            var model = new CrearSocioViewModel
            {
                Id = socio.Id,
                Nombre = socio.Nombre,
                Apellido = socio.Apellido,
                Dni = socio.Dni,
                Email = socio.Email,
                Telefono = socio.Telefono,
                FechaIngreso = socio.FechaIngreso,
                Domicilio = socio.Domicilio,
                Localidad = socio.Localidad,
                Actividades = actividadesVM,
            };
            return View(model);
        }

        // POST: Socio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,CrearSocioViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var socioExistente = await _context.Socios
                    .Include(s => s.SocioActividades)
                    .FirstOrDefaultAsync(s => s.Id == id && s.UsuarioId == userId);
                if (socioExistente == null)
                {
                    return NotFound();
                }
                
                model.Actividades = model.Actividades ?? new List<SocioActividadViewModel>();
                socioExistente.SocioActividades.Clear();
                foreach (var actividad in model.Actividades.Where(a => a.Seleccionado))
                {
                    socioExistente.SocioActividades.Add(new SocioActividad
                    {
                        ActividadId = actividad.ActividadId,
                        DiasPorSemana = actividad.DiasPorSemana,
                    });
                }

                
                socioExistente.Nombre = model.Nombre;
                socioExistente.Apellido = model.Apellido;
                socioExistente.Dni = model.Dni;
                socioExistente.Telefono = model.Telefono;
                socioExistente.Email = model.Email;
                socioExistente.FechaIngreso = model.FechaIngreso;
                socioExistente.Domicilio = model.Domicilio;
                socioExistente.Localidad = model.Localidad;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        // GET: Socio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var socio = await _context.Socios
                .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == userId);
            if (socio == null)
            {
                return NotFound();
            }

            return View(socio);
        }

        // POST: Socio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var socio = await _context.Socios.FirstOrDefaultAsync(s => s.Id == id && s.UsuarioId == userId);
            if (socio != null)
            {
                _context.Socios.Remove(socio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SocioExists(int id)
        {
            return _context.Socios.Any(e => e.Id == id);
        }
    }
}
