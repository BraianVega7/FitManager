using System.ComponentModel.DataAnnotations;

namespace FitManager.ViewModels
{
    public class CrearSocioViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public int Dni { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; } = DateTime.Today;

        [Required]
        public string Domicilio { get; set; }

        [Required]
        public string Localidad { get; set; }

        public string UsuarioId { get; set; } = string.Empty;

        public List<SocioActividadViewModel> Actividades { get; set; } = new();
    }
}
