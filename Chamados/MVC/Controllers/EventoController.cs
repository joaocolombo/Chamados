using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Chunks.Generators;
using MVC.ViewModel;
using MVC.ViewModel.Evento;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVC.Controllers
{
    public class EventoController : Controller
    {
        private string url = "http://10.1.0.4";

        [HttpGet]
        public IActionResult Novo(int id)
        {
            return PartialView("_AdicionarEvento", new AdicionarEventoViewModel() { ChamadoId = id });
        }

        private HttpResponseMessage PutMethodEvento(string uri, string uriParametros, string json)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + uri);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                return client.PutAsync(url + uriParametros, new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;


            }
        }


        private AdicionarEventoViewModel ConvertEventoToViewModel(Evento evento)
        {
            return new AdicionarEventoViewModel()
            {
                Descricao = evento.Descricao,
                Status = evento.Status,
                Abertura = evento.Abertura,
                Atendente = evento.Atendente,
                Codigo = evento.Codigo,
                FilaId = evento.Codigo

            };
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
                    var eventoVM = ConvertEventoToViewModel(JsonConvert.DeserializeObject<Evento>(stringData));
                    return View(eventoVM);
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
        public IActionResult AlterarDescricao(AlterarDescricaoViewModel alterarDescricao)
        {
            {
                var json = JsonConvert.SerializeObject(new Atendente()
                {
                    Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
                });
                var response = PutMethodEvento("/api/Evento/AlterarDescricao/{descricao}/{id}",
                    "/api/Evento/AlterarDescricao/" + alterarDescricao.Descricao + "/" +
                    alterarDescricao.Id, json);

                if (response.IsSuccessStatusCode)
                {
                    var eventoVM = ConvertEventoToViewModel(JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result));
                    return PartialView("~/Views/Evento/_Visualizar.cshtml", eventoVM);
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return View(response.Content.ReadAsStringAsync().Result);



            }
        }
        [HttpPost]
        public IActionResult AlterarStatus(AlterarStatusViewModel alterarStatus)
        {
            {
                var json = JsonConvert.SerializeObject(new Atendente()
                {
                    Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
                });
                var response = PutMethodEvento("/api/Evento/AlterarStatus/{status}/{id}",
                    "/api/Evento/AlterarStatus/" + alterarStatus.Status + "/" +
                    alterarStatus.Id, json);

                if (response.IsSuccessStatusCode)
                {
                    var eventoVM = ConvertEventoToViewModel(JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result));
                    return PartialView("~/Views/Evento/_Visualizar.cshtml", eventoVM);
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return View(response.Content.ReadAsStringAsync().Result);


            }
        }

        [HttpPost]
        public IActionResult DirecionarNovoEvento(AdicionarEventoViewModel adicionarEvento)
        {
            if (adicionarEvento.Direcao == 1)
            {
                return Novo(adicionarEvento);
            }
            else if (adicionarEvento.Direcao == 2)
            {
                return AlterarFila(adicionarEvento);

            }
            else
            {
                return EncaminharOutroAtendente(adicionarEvento);
            }
        }
        [HttpPost]
        public IActionResult AlterarFila(AdicionarEventoViewModel eventoViewModel)
        {

            var atendente = new Atendente()
            {
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
            };

            var evento = new Evento()
            {
                Descricao = eventoViewModel.Descricao,
                Atendente = new Atendente()
                {
                    Nome = eventoViewModel.Atendente.Nome
                },
                Status = "ENCAMINHAR"

            };
            
            var fila = new Fila() { Codigo = eventoViewModel.FilaId };
            List<object> lista = new List<object>();
            lista.Add(fila);
            lista.Add(atendente);
            lista.Add(evento);
            var json = JsonConvert.SerializeObject(lista);

            var response =
                PutMethodEvento("/api/Chamado/AlterarFila/{id}",
                    "/api/Chamado/AlterarFila/" + eventoViewModel.ChamadoId, json);

            if (response.IsSuccessStatusCode)
            {
                var eventoVM = ConvertEventoToViewModel(JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result));
                return PartialView("~/Views/Evento/_Evento.cshtml", eventoVM);
            }
            if (response.ReasonPhrase.Equals("Unprocessable Entity"))
            {
                return StatusCode(422, response.Content.ReadAsStringAsync().Result);
            }
            return StatusCode(500, response.Content.ReadAsStringAsync().Result);


        }
        [HttpPost]
        public IActionResult EncaminharOutroAtendente(AdicionarEventoViewModel eventoViewModel)
        {

            var atendente=new Atendente()
            {
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
            };

            var evento = new Evento()
            {
                Descricao = eventoViewModel.Descricao,
                Atendente = new Atendente()
                {
                    Nome = eventoViewModel.NomeAtendenteNovo
                },
                Status = "ENCAMINHAR"

            };
            List<object> lista = new List<object>();
            lista.Add(atendente);
            lista.Add(evento);
            var json = JsonConvert.SerializeObject(lista);

            var response = PutMethodEvento("/api/Chamado/Encaminhar/{id}", "/api/Chamado/Encaminhar/" + eventoViewModel.ChamadoId, json);

            if (response.IsSuccessStatusCode)
            {
                var eventoVM = ConvertEventoToViewModel(JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result));
                return PartialView("~/Views/Evento/_Evento.cshtml", eventoVM);
            }
            if (response.ReasonPhrase.Equals("Unprocessable Entity"))
            {
                return StatusCode(422, response.Content.ReadAsStringAsync().Result);
            }
            return StatusCode(500, response.Content.ReadAsStringAsync().Result);

        }
        [HttpPost]
        public IActionResult Novo(AdicionarEventoViewModel adicionarEvento)
        {
            if (string.IsNullOrEmpty(adicionarEvento.NomeAtendenteNovo))
            {
                adicionarEvento.NomeAtendenteNovo = adicionarEvento.Atendente.Nome;
            }
            var atendente = new Atendente()
            {
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
            };
            var evento = new Evento()
            {
                Descricao = adicionarEvento.Descricao,
                Atendente = atendente,
                Status = adicionarEvento.Status

            };


            List<object> lista = new List<object>();
            lista.Add(adicionarEvento.ChamadoId);
            lista.Add(JsonConvert.SerializeObject(atendente));
            lista.Add(JsonConvert.SerializeObject(evento));

            var json = JsonConvert.SerializeObject(lista);
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + "/api/Evento/Adicionar");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.PostAsync(url + "/api/Evento/Adicionar",
                            new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;

                if (response.IsSuccessStatusCode)
                {
                    var eventoVM =
                        ConvertEventoToViewModel(
                            JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result));
                    return PartialView("~/Views/Evento/_Evento.cshtml", eventoVM);
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
