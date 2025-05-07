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
    }
}
