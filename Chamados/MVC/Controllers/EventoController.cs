using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Chunks.Generators;
using MVC.ViewModel;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVC.Controllers
{
    public class EventoController : Controller
    {
        private string url = "http://10.1.0.4";


        public IActionResult Novo(string id)
        {
            return PartialView("_AdicionarEvento", new AdicionarEventoViewModel() { ChamadoId = id });
        }

        [HttpPost]
        public IActionResult Novo(AdicionarEventoViewModel AdicionarEvento)
        {
            if (string.IsNullOrEmpty(AdicionarEvento.NomeAtendenteNovo))
            {
                AdicionarEvento.NomeAtendenteNovo = AdicionarEvento.NomeAtendenteAtual;
            }
            
            var evento = new Evento()
            {
                Descricao = AdicionarEvento.Descricao,
                Atendente = new Atendente()
                {
                    Nome = AdicionarEvento.NomeAtendenteNovo
                },
                Status = AdicionarEvento.Status

            };
            var atendente = new Atendente(){Nome = AdicionarEvento.NomeAtendenteAtual};

            List<object> lista = new List<object>();
            lista.Add(AdicionarEvento.ChamadoId);
            lista.Add(JsonConvert.SerializeObject(atendente));
            lista.Add(JsonConvert.SerializeObject(evento));

            var json = JsonConvert.SerializeObject(lista);
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + "/api/Evento/Adicionar");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.PostAsync(url + "/api/Evento/Adicionar", new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;
                var id = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Visualizar", "Home", new{id=AdicionarEvento.ChamadoId});
                }
                var result = response.Content.ReadAsStringAsync().Result;
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    ViewData["Error"] = result;
                    return View();
                }
                return RedirectToAction("Visualizar", "Home", new { id = AdicionarEvento.ChamadoId });
            }
        }
    }
}
