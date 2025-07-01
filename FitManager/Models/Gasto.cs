using System.ComponentModel.DataAnnotations;

namespace FitManager.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La descripción del gasto es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;
        [Required(ErrorMessage = "El monto del gasto es obligatorio")]
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string UsuarioId { get; set; } = string.Empty;
    }
}
