using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyect.Models
{
    public class CotizacionViewModel
    {
        public Welcome CotizacionActual { get; set; }
        public List<Cotizaciones> Historial { get; set; }
    }
}