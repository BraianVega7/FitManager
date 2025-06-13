using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FitManager.Models
{
    public class Actividad
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la actividad es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        public string UsuarioId { get; set; } = string.Empty;

        [ValidateNever]
        public ICollection<ActividadPrecio> PreciosPorDia { get; set; } = new List<ActividadPrecio>();
        public ICollection<SocioActividad> SocioActividades { get; set; } = new List<SocioActividad>();
        public ICollection<EntrenadorActividad> EntrenadorActividades { get; set; } = new List<EntrenadorActividad>();
    }
}
