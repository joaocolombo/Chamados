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

        [HttpGet]
        public IActionResult Novo(string id)
        {
            return PartialView("_AdicionarEvento", new AdicionarEventoViewModel() { ChamadoId = id });
        }

        [HttpGet]
        public IActionResult Visualizar(string id)


        {

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + "/api/Evento/BuscarPorId/{id}");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.GetAsync(url + "/api/Evento/BuscarPorId/" + id).Result;
                var stringData = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    return View(JsonConvert.DeserializeObject<Evento>(stringData));
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    ViewData["Error"] = result;
                    return View("error");
                }
                return View("error");

            }

        }

        [HttpGet]
        public IActionResult RetornoVisualizar(Evento evento)
        {
            return View("Visualizar", evento);
        }

        [HttpGet]
        public IActionResult AlterarDescricao(string id, string descricao)
        {
            return PartialView("_AlterarDescricao", new AlterarDescricaoViewModel() { Id = id, Descricao = descricao });
        }

        [HttpGet]
        public IActionResult AlterarStatus(string id, string status)
        {
            return PartialView("_AlterarStatus", new AlterarStatusViewModel() { Id = id, Status = status });
        }
        [HttpPost]
        public IActionResult EncaminharN2(AlterarDescricaoViewModel alterarDescricao)
        {
            {
                var json = JsonConvert.SerializeObject(new Atendente() { Nome = alterarDescricao.NomeAtendente });

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(url + "/api/Evento/AlterarDescricao/{descricao}/{id}");
                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    var response =
                        client.PutAsync(
                                url + "/api/Evento/AlterarDescricao/" + alterarDescricao.Descricao + "/" +
                                alterarDescricao.Id, new StringContent(json, Encoding.UTF8, "application/json"))
                            .Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var evento = JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result);
                        return RetornoVisualizar(evento);
                    }
                    if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                    {
                        return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                    }
                    return View(response.Content.ReadAsStringAsync().Result);

                }
            }
        }

        [HttpPost]
        public IActionResult AlterarDescricao(AlterarDescricaoViewModel alterarDescricao)
        {
            {
                var json = JsonConvert.SerializeObject(new Atendente() { Nome = alterarDescricao.NomeAtendente });

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(url + "/api/Evento/AlterarDescricao/{descricao}/{id}");
                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    var response =
                        client.PutAsync(
                                url + "/api/Evento/AlterarDescricao/" + alterarDescricao.Descricao + "/" +
                                alterarDescricao.Id, new StringContent(json, Encoding.UTF8, "application/json"))
                            .Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var evento = JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result);
                        return RetornoVisualizar(evento);
                    }
                    if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                    {
                        return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                    }
                    return View(response.Content.ReadAsStringAsync().Result);


                }
            }
        }
        [HttpPost]
        public IActionResult AlterarStatus(AlterarStatusViewModel alterarStatus)
        {
            {
                var json = JsonConvert.SerializeObject(new Atendente() { Nome = alterarStatus.NomeAtendente });

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(url + "/api/Evento/AlterarStatus/{status}/{id}");
                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    var response =
                        client.PutAsync(
                                url + "/api/Evento/AlterarStatus/" + alterarStatus.Status + "/" +
                                alterarStatus.Id, new StringContent(json, Encoding.UTF8, "application/json"))
                            .Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var evento = JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result);
                        return RetornoVisualizar(evento);
                    }
                    if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                    {
                        return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                    }
                    return View(response.Content.ReadAsStringAsync().Result);

                }
            }
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
            var atendente = new Atendente() { Nome = AdicionarEvento.NomeAtendenteAtual };

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

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Visualizar", "Home", new { id = AdicionarEvento.ChamadoId });
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return View(response.Content.ReadAsStringAsync().Result);

            }
        }
    }
}
