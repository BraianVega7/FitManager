using System.ComponentModel.DataAnnotations;

namespace FitManager.Models
{
    public class Socio
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del socio es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido del socio es obligatorio")]
        [Display(Name = "Apellido")]
        public string Apellido {get; set; }
        [Required(ErrorMessage = "El dni del socio es obligatorio")]
        [Display(Name = "Dni")]
        public int Dni { get; set; }
        [Required(ErrorMessage = "El telefono del socio es obligatorio")]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El email del socio es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un email válido")]
        [Display(Name = "Email")]
        public string Email {  get; set; }
        [Required(ErrorMessage = "La fecha de ingreso es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; }
        [Required(ErrorMessage = "La localidad del socio es obligatorio")]
        [Display(Name = "Localidad")]
        public string Localidad { get; set; }
        [Required(ErrorMessage = "El domicilio del socio es obligatorio")]
        [Display(Name = "Domicilio")]
        public string Domicilio {  get; set; }
        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
        public string UsuarioId { get; set; } = string.Empty;
        public ICollection<SocioActividad> SocioActividades { get; set; } = new List<SocioActividad>();
    }
}
