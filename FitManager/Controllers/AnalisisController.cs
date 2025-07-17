using FitManager.Data;
using FitManager.Models;
using FitManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitManager.Controllers
{
    public class AnalisisController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AnalisisController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var socios =  await _context.Socios
                .Where(s => s.UsuarioId == userId)
                .ToListAsync();

            var activos = socios.Count(s => s.Activo);
            var inactivos = socios.Count(s => !s.Activo);

            var totalSueldos = await _context.Entrenadores
                .Where(e => e.UsuarioId == userId)
                .SumAsync(e => e.SueldoMensual);

            var totalIngresos =await  _context.Pago
                .Where(p => p.FechaPago.Month == DateTime.Now.Month && p.FechaPago.Year == DateTime.Now.Year && p.Socio.UsuarioId == userId)
                .SumAsync(p => p.Monto);
            var totalGastos = await _context.Gastos
                .Where(g => g.Fecha.Month == DateTime.Now.Month && g.Fecha.Year == DateTime.Now.Year && g.UsuarioId == userId)
                .SumAsync(g => g.Monto);

            var totalEgresos = totalSueldos + totalGastos;

            var viewModel = new CrearAnalisisViewModel
            {
                CantidadActivos = activos,
                CantidadInactivos = inactivos,
                TotalIgresos = totalIngresos,
                TotalEgresos = totalEgresos,
                MesActual = DateTime.Now
            };
            return View(viewModel);
        }
    }
}
