using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyect.Models
{
    public class HomeIndexViewModel
    {
        public List<Programas> TodosLosProgramas { get; set; }  // Todos los programas
        public List<ProgramacionHoraria> ProgramacionHoy { get; set; }  // Grilla del día
        public TimeSpan HoraActual { get; set; }  // Para marcar el que está al aire
    }
}