using FitManager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {           

        }

        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<ActividadPrecio> ActividadPrecios { get; set; }
        public DbSet<Socio> Socios { get; set; }
        public DbSet<Entrenador> Entrenadores { get; set; }
        public DbSet<EntrenadorActividad> EntrenadorActividades { get; set; }
        public DbSet<Pago> Pago { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
    }
}
