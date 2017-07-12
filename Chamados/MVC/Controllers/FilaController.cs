using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModel.Evento;
using MVC.ViewModel.Fila;
using MVC.ViewModel.Home;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class FilaController : Controller
    {
        private string url = "http://10.1.0.4";

        private HttpResponseMessage GetApi(string caminho, string parametrosUrl)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url + caminho);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                return client.GetAsync(parametrosUrl).Result;

            }
        }

        private HttpResponseMessage PutApi(string caminho, string parametrosUrl, string json)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + caminho);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                return client.PutAsync(url + parametrosUrl,
                             new StringContent(json, Encoding.UTF8, "application/json")).Result;

            }
        }

        public IActionResult Index()
        {
            var response = GetApi("/Api/fila/BuscarTodos/Cabecarios", "/Api/fila/BuscarTodos/Cabecarios");
            var stringData = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                return View(JsonConvert.DeserializeObject<List<FilaViewModel>>(stringData));
            }
            if (response.ReasonPhrase.Equals("Unprocessable Entity"))
            {
                return StatusCode(422, response.Content.ReadAsStringAsync().Result);
            }
            return StatusCode(500, response.Content.ReadAsStringAsync().Result);

        }

        public IActionResult ListaChamadosFila(int id)
        {
            var response = GetApi("/Api/fila/BuscarPorId/{id}", "/Api/fila/BuscarPorId/" + id);
            var stringData = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                return View("ListaChamadosFila", JsonConvert.DeserializeObject<FilaViewModel>(stringData));
            }
            if (response.ReasonPhrase.Equals("Unprocessable Entity"))
            {
                return StatusCode(422, response.Content.ReadAsStringAsync().Result);
            }
            return StatusCode(500, response.Content.ReadAsStringAsync().Result);
        }

        public IActionResult VisualizarChamado(int id)
        {
            var response = GetApi("/Api/Chamado/BuscarPorId/{id}", "/Api/Chamado/BuscarPorId/" + id);
            var stringData = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                return View("Visualizar", JsonConvert.DeserializeObject<ChamadoViewModel>(stringData));
            }
            if (response.ReasonPhrase.Equals("Unprocessable Entity"))
            {
                return StatusCode(422, response.Content.ReadAsStringAsync().Result);
            }
            return StatusCode(500, response.Content.ReadAsStringAsync().Result);
        }
        [HttpGet]
        public IActionResult AssumirChamado(int id)
        {

            return PartialView("_AssumirChamado",
                new AdicionarEventoViewModel() { ChamadoId = id});

        }
        [HttpPost]
        public IActionResult AssumirChamado(AdicionarEventoViewModel adicionarEventoViewModel)
        {
            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
                
            });
            var response = PutApi("/Api/Chamado/AssumirChamado/{id}", "/Api/Chamado/AssumirChamado/" 
                + adicionarEventoViewModel.ChamadoId, json);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Index", "Home");
            }
            if (response.ReasonPhrase.Equals("Unprocessable Entity"))
            {
                return StatusCode(422, response.Content.ReadAsStringAsync().Result);
            }
            return StatusCode(500, response.Content.ReadAsStringAsync().Result);

        }
    }

}