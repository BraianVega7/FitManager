using System.ComponentModel.DataAnnotations;

namespace FitManager.Models
{
    public class Entrenador
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del entrenador es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido del entrenador es obligatorio")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El email del entrenador es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El teléfono del entrenador es obligatorio")]
        [Phone(ErrorMessage = "El teléfono no es válido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El sueldo es obligatorio")]
        [Range(0, double.MaxValue)]
        public decimal SueldoMensual { get; set; }

        public string UsuarioId { get; set; } = string.Empty;
        public ICollection<EntrenadorActividad> EntrenadorActividades { get; set; } = new List<EntrenadorActividad>();
    }
}
