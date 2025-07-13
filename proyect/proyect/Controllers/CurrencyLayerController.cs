using proyect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace proyect.Controllers
{
    public class CurrencyLayerController : Controller
    {
        private VozDelEsteEntities db = new VozDelEsteEntities();

        // GET: CurrencyLayer
        public ActionResult Index()
        {
            return View();
        }




    public async Task<ActionResult> MostrarCotizacion()
    {
        string url = "http://api.currencylayer.com/live?access_key=38eff056ef68d1ba59168de1669232a3&currencies=EUR";

        Welcome data;

        using (HttpClient client = new HttpClient())
        {
            string json = await client.GetStringAsync(url);
            data = Welcome.FromJson(json);

            if (data.Success)
            {
                // Guardar en la base si no existe para esa fecha
                DateTime hoy = DateTime.Today;

                bool existe = db.Cotizaciones.Any(c =>
                    c.Fecha == hoy && c.TipoMoneda == "EUR");

                if (!existe)
                {
                    db.Cotizaciones.Add(new Cotizaciones
                    {
                        Fecha = hoy,
                        TipoMoneda = "EUR",
                        Valor = (decimal)data.Quotes.UsdEur
                    });
                    db.SaveChanges();
                }
            }
        }

        var historial = db.Cotizaciones
                          .Where(c => c.TipoMoneda == "EUR")
                          .OrderBy(c => c.Fecha)
                          .ToList();

        var vm = new CotizacionViewModel
        {
            CotizacionActual = data,
            Historial = historial
        };

        return View(vm);
    }




        public ActionResult GraficaCotizacion()
        {
            using (var db = new VozDelEsteEntities())
            {
                var cotizaciones = db.Cotizaciones
                    .Where(c => c.TipoMoneda == "EUR")
                    .OrderBy(c => c.Fecha)
                    .ToList();

                return View(cotizaciones);
            }
        }



    }
}