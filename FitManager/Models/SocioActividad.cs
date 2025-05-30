using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitManager.Models
{
    public class SocioActividad
    {
        public int Id { get; set; }
        public int ActividadId {  get; set; }
        [ForeignKey("ActividadId")]
        public Actividad Actividad { get; set; }
        public int SocioId {  get; set; }
        [ForeignKey("SocioId")]
        public Socio Socio { get; set; }
        [Required(ErrorMessage = "Los dias por semana es obligatorio")]
        [Range(1, 5, ErrorMessage = "Los días por semana deben estar entre 1 y 5")]
        public int DiasPorSemana { get; set; }
    }
}
