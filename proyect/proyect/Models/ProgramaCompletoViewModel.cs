using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyect.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

    public class ProgramaCompletoViewModel
    {
        public int? ProgramacionHorariaId { get; set; }

        [Required(ErrorMessage = "El nombre del programa es obligatorio.")]
        public Programas Programa { get; set; }  // ⚠️ Si validás campos internos, lo ideal sería desglosarlos en el ViewModel

        public int? ConductorIdSeleccionado { get; set; }

        public IEnumerable<SelectListItem> ConductoresDisponibles { get; set; }

        [Required(ErrorMessage = "El día de la semana es obligatorio.")]
        public string DiaSemana { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de finalización es obligatoria.")]
        [CustomValidation(typeof(ProgramaCompletoViewModel), nameof(ValidarRangoHorario))]
        public TimeSpan HoraFin { get; set; }

        public HttpPostedFileBase ImagenFile { get; set; }

        // ✅ Validación personalizada para verificar que la hora de fin sea posterior a la de inicio
        public static ValidationResult ValidarRangoHorario(TimeSpan horaFin, ValidationContext context)
        {
            var instance = (ProgramaCompletoViewModel)context.ObjectInstance;
            if (horaFin <= instance.HoraInicio)
            {
                return new ValidationResult("La hora de fin debe ser posterior a la hora de inicio.");
            }
            return ValidationResult.Success;
        }

    }
}
