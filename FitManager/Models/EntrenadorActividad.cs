using System.ComponentModel.DataAnnotations.Schema;

namespace FitManager.Models
{
    public class EntrenadorActividad
    {
        public int Id { get; set; }
        [ForeignKey("EntrenadorId")]
        public int EntrenadorId { get; set; }
        public Entrenador? Entrenador { get; set; }
        [ForeignKey("ActividadId")]
        public int ActividadId { get; set; }
        public Actividad? Actividad { get; set; }
    }
}
