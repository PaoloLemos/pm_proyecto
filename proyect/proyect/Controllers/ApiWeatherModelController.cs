using Newtonsoft.Json;
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
    public class ApiWeatherModelController : Controller
    {
        private readonly string apiKey = "352197478cb79204e7cffd1537f3f77c";

        public async Task<ActionResult> clima()
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat=-34.9&lon=-54.95&appid={apiKey}&units=metric&lang=es";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url);
                    var clima = JsonConvert.DeserializeObject<ApiWeatherModel>(response);
                    return View(clima);
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.Error = "No se pudo obtener el clima. Detalles: " + ex.Message;
                    return View();
                }
            }
        }
    }
}