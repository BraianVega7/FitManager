using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitManager.Models
{
    public class ActividadPrecio
    {
        public int Id { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Días por semana debe estar entre 1 y 5")]
        public int DiasPorSemana { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal Precio { get; set; }
        [Required]
        public int ActividadId { get; set; }
        [ForeignKey("ActividadId")]
        public Actividad Actividad { get; set; }
    }
}
