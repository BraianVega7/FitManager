using System.Globalization;

namespace FitManager.ViewModels
{
    public class CrearAnalisisViewModel
    {
        public int CantidadActivos { get; set; }
        public int CantidadInactivos { get; set; }
        
        public decimal TotalIgresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal TotalGanancias => TotalIgresos - TotalEgresos;
        public DateTime MesActual { get; set; }
        public string MesAnalizado => MesActual.ToString("MMMM yyyy", new CultureInfo("es-ES"));
    }
}
