using FitManager.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace FitManager.ViewModels
{
    public class SocioActividadViewModel
    {
        public int ActividadId { get; set; }
        public string NombreActividad { get; set; }
        public bool Seleccionado {  get; set; }
        [Range(1, 5)]
        public int DiasPorSemana {  get; set; }

    }
}
