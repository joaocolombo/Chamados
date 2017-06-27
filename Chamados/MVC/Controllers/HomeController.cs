using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private string url = "http://10.1.0.4";


        public IActionResult Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url + "/api/chamado/buscarporatendente/joao/1");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response = client.GetAsync("/api/chamado/buscarporatendente/joao/1").Result;
                var stringData = response.Content.
                    ReadAsStringAsync().Result;
                var chamado = JsonConvert.DeserializeObject<List<Chamado>>(stringData);
                return View(chamado);
            }
        }
        [HttpGet]
        public IActionResult Novo()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url + "/api/filial/buscarfilial");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response = client.GetAsync("/api/filial/buscarfilial").Result;
                var stringData = response.Content.
                    ReadAsStringAsync().Result;
                var filialis = JsonConvert.DeserializeObject<List<Filial>>(stringData);
                filialis = filialis.OrderBy(x => x.Nome).ToList();
                ViewBag.Filiais = new SelectList(filialis, "Codigo", "Nome");


                return PartialView("_Novo");
            }

        }
        [HttpPost]
        public IActionResult Novo(Chamado chamado)
        {
            return View("About");
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
