using FitManager.Data;
using FitManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitManager.Controllers
{
    public class GastoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GastoController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gastosDelMes = await _context.Gastos
                .Where(g => g.Fecha.Month == DateTime.Now.Month && g.Fecha.Year == DateTime.Now.Year && g.UsuarioId == userId)
                .ToListAsync();
            return View(gastosDelMes);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                gasto.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                gasto.Fecha = DateTime.Now;
                _context.Gastos.Add(gasto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var gastosDelMes = await _context.Gastos
                .Where(g => g.Fecha.Month == DateTime.Now.Month && g.Fecha.Year == DateTime.Now.Year)
                .ToListAsync();
            return View(gastosDelMes);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null || gasto.UsuarioId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            return View(gasto);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Gasto gasto)
        {
            if (id != gasto.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    gasto.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    _context.Update(gasto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Gastos.Any(e => e.Id == gasto.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gasto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var gasto = await _context.Gastos.FirstOrDefaultAsync(g =>
                g.Id == id && g.UsuarioId == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (gasto == null)
                return NotFound();

            return View(gasto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto != null && gasto.UsuarioId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                _context.Gastos.Remove(gasto);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

