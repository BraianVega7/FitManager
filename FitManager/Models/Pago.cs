using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitManager.Models
{
    public class Pago
    {
        public int Id { get; set; }

        [Required]
        public int SocioId { get; set; }
        [ForeignKey("SocioId")]
        public Socio Socio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pago")]
        public DateTime FechaPago { get; set; }
        [Display(Name = "Monto Calculado")]
        public decimal Monto { get; set; }

    }
}
