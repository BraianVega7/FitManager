using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitManager.Data;
using FitManager.Models;
using System.Security.Claims;
using FitManager.ViewModels;

namespace FitManager.Controllers
{
    public class PagoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sociosActivos = await _context.Socios
                .Include(s => s.SocioActividades)
                    .ThenInclude(sa => sa.Actividad)
                        .ThenInclude(a => a.PreciosPorDia)
                .Where(s => s.Activo && s.UsuarioId == userId)
                .ToListAsync();

            var pagosDelMes = await _context.Pago
                .Where(p => p.FechaPago.Month == DateTime.Now.Month && p.FechaPago.Year == DateTime.Now.Year)
                .ToListAsync();

            var viewModels = sociosActivos.Select(socio =>
            {
                var actividades = socio.SocioActividades.Select(sa =>
                {
                    var precio = sa.Actividad.PreciosPorDia
                        .FirstOrDefault(p => p.DiasPorSemana == sa.DiasPorSemana)?.Precio ?? 0;

                    return new ActividadPago
                    {
                        nombreActividad = sa.Actividad.Nombre,
                        diasPorSemana = sa.DiasPorSemana,
                        precio = precio
                    };
                }).ToList();

                var total = actividades.Sum(a => a.precio);

                var yaPago = pagosDelMes.Any(p => p.SocioId == socio.Id);

                return new CrearPagoViewModel
                {
                    SocioId = socio.Id,
                    Nombre = socio.Nombre,
                    Apellido = socio.Apellido,
                    Dni = socio.Dni,
                    Actividades = actividades,
                    Total = total,
                    YaPagado = yaPago,
                    FechaPago = DateTime.Now
                };
            }).ToList();

            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPago(int socioId)
        {
            var socio = await _context.Socios
                .Include(s => s.SocioActividades)
                    .ThenInclude(sa => sa.Actividad)
                        .ThenInclude(a => a.PreciosPorDia)
                .FirstOrDefaultAsync(s => s.Id == socioId);

            if (socio == null)
                return NotFound();

            decimal total = 0;

            foreach (var sa in socio.SocioActividades)
            {
                var precio = sa.Actividad.PreciosPorDia
                    .FirstOrDefault(p => p.DiasPorSemana == sa.DiasPorSemana);
                if (precio != null)
                    total += precio.Precio;
            }

            var pago = new Pago
            {
                SocioId = socioId,
                FechaPago = DateTime.Now,
                Monto = total
            };

            _context.Pago.Add(pago);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> CancelarPago(int socioId)
        {
            var pago = await _context.Pago
                .FirstOrDefaultAsync(p => p.SocioId == socioId && 
                p.FechaPago.Month == DateTime.Now.Month && 
                p.FechaPago.Year == DateTime.Now.Year);
            if (pago == null)
            {
                return NotFound();
            }
            else
            {
                _context.Pago.Remove(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

