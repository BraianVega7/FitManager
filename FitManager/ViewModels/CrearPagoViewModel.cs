using FitManager.Models;

namespace FitManager.ViewModels
{
    public class ActividadPago
    {
        public string nombreActividad { get; set; }
        public int diasPorSemana { get; set; }
        public decimal precio { get; set; }
    }

    public class CrearPagoViewModel
    {
        public int SocioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int Dni { get; set; }
        public List<ActividadPago> Actividades { get; set; } = new List<ActividadPago>();
        public decimal Total { get; set; }
        public bool YaPagado { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.Now;
    }
}
