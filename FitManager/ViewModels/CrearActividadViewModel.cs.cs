using FitManager.Models;

namespace FitManager.ViewModels
{
    public class CrearActividadViewModel
    {
        public Actividad Actividad { get; set; } = new Actividad();
        public List<PrecioPorDiaViewModel> Precios { get; set; } = new();
    }

    public class PrecioPorDiaViewModel
    {
        public int DiasPorSemana { get; set; }
        public decimal Precio { get; set; }
    }
}