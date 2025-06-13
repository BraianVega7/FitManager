using FitManager.Models;
using System.ComponentModel.DataAnnotations;

namespace FitManager.ViewModels
{
    public class CrearEntrenadorViewModel
    {
        public Entrenador Entrenador { get; set; } = new Entrenador();

        public List<Actividad> ActividadDisponible { get; set; } = new List<Actividad>();

        public List <int> ActividadIdSeleccionadas { get; set; } = new List<int>();
    }
}
