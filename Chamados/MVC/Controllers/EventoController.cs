using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MVC.Interfaces;
using MVC.Mapper;
using MVC.ViewModel.Evento;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVC.Controllers
{
    public class EventoController : Controller
    {
        private readonly IConsumirApi _iConsumirApi;

        public EventoController(IConsumirApi iConsumirApi)
        {
            _iConsumirApi = iConsumirApi;
        }

        [HttpGet]
        public IActionResult Novo(int id)
        {
            return PartialView("_AdicionarEvento", new AdicionarEventoViewModel() { ChamadoId = id });
        }


        [HttpGet]
        public IActionResult Visualizar(string id)
        {
            var response = _iConsumirApi.GetMethod("/api/Evento/BuscarPorId/{id}", "/api/Evento/BuscarPorId/" + id);
            if (response.IsSuccessStatusCode)
                return View(EventoTo.AdicionarEventoViewModel(JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result)));

            ViewData["Error"] = response.Content.ReadAsStringAsync().Result;
            return View("error");
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
            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            });
            var response = _iConsumirApi.PutMethod("/api/Evento/AlterarDescricao/{descricao}/{id}",
                "/api/Evento/AlterarDescricao/" + alterarDescricao.Descricao + "/" +
                alterarDescricao.Id, json);

            if (response.IsSuccessStatusCode)
                return PartialView("~/Views/Evento/_Visualizar.cshtml", EventoTo.AdicionarEventoViewModel(JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult AlterarStatus(AlterarStatusViewModel alterarStatus)
        {
            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
            });
            var response = _iConsumirApi.PutMethod("/api/Evento/AlterarStatus/{status}/{id}",
                "/api/Evento/AlterarStatus/" + alterarStatus.Status + "/" +
                alterarStatus.Id, json);
            if (response.IsSuccessStatusCode)
                return PartialView("~/Views/Evento/_Visualizar.cshtml", EventoTo.AdicionarEventoViewModel(JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult DirecionarNovoEvento(AdicionarEventoViewModel adicionarEvento)
        {
            switch (adicionarEvento.Direcao)
            {
                case 1:
                    return Novo(adicionarEvento);
                case 2:
                    return AlterarFila(adicionarEvento);
                default:
                    return EncaminharOutroAtendente(adicionarEvento);
            }
        }
        [HttpPost]
        public IActionResult AlterarFila(AdicionarEventoViewModel eventoViewModel)
        {
            var atendente = new Atendente()
            {
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            };
            var evento = new Evento(0, eventoViewModel.Descricao, new Atendente() { Nome = eventoViewModel.Atendente.Nome });
            evento.Status="ENCAMINHAR";

            var fila = new Fila() { Codigo = eventoViewModel.FilaId };
            List<object> lista = new List<object>() { fila, atendente, evento };

            var response = _iConsumirApi.PutMethod("/api/Chamado/AlterarFila/{id}",
                    "/api/Chamado/AlterarFila/" + eventoViewModel.ChamadoId, JsonConvert.SerializeObject(lista));

            if (response.IsSuccessStatusCode)
                return PartialView("~/Views/Evento/_Evento.cshtml", EventoTo.AdicionarEventoViewModel(JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }
        [HttpPost]
        public IActionResult EncaminharOutroAtendente(AdicionarEventoViewModel eventoViewModel)
        {
            var atendente = new Atendente()
            {
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            };
            var evento = new Evento(0, eventoViewModel.Descricao,
                new Atendente() { Nome = eventoViewModel.NomeAtendenteNovo });
            evento.Status="ENCAMINHAR";
            List<object> lista = new List<object>() { atendente, evento };
            var response = _iConsumirApi.PutMethod("/api/Chamado/Encaminhar/{id}", "/api/Chamado/Encaminhar/" + eventoViewModel.ChamadoId, JsonConvert.SerializeObject(lista));
            if (response.IsSuccessStatusCode)
                return PartialView("~/Views/Evento/_Evento.cshtml", EventoTo.AdicionarEventoViewModel(JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
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
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            };
            var evento = new Evento(0, adicionarEvento.Descricao, atendente)
            {
                MinutosPrevistos = adicionarEvento.MinutosPrevistos,
                MinutosRealizados = adicionarEvento.MinutosRealizados
            };
            evento.Status=adicionarEvento.Status;

            List<object> lista = new List<object>() { adicionarEvento.ChamadoId, atendente, evento };
            var response = _iConsumirApi.PostMethod("/api/Evento/Adicionar", JsonConvert.SerializeObject(lista));
            if (response.IsSuccessStatusCode)
                return PartialView("~/Views/Evento/_Evento.cshtml", EventoTo.AdicionarEventoViewModel(
                        JsonConvert.DeserializeObject<Evento>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }
    }
}
