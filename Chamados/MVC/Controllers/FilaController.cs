using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Interfaces;
using MVC.ViewModel.Evento;
using MVC.ViewModel.Fila;
using MVC.ViewModel.Home;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize(Roles = "asd")]
    public class FilaController : Controller
    {
        private readonly IConsumirApi _iConsumirApi;

        public FilaController(IConsumirApi iConsumirApi)
        {
            _iConsumirApi = iConsumirApi;
        }

        public IActionResult Index()
        {
            var response = _iConsumirApi.GetMethod("/Api/fila/BuscarTodos/Cabecarios",
                "/Api/fila/BuscarTodos/Cabecarios");
            if (response.IsSuccessStatusCode)
                return
                    View(JsonConvert.DeserializeObject<List<FilaViewModel>>(response.Content.ReadAsStringAsync().Result));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);


        }

        public IActionResult ListaChamadosFila(int id)
        {
            var response = _iConsumirApi.GetMethod("/Api/fila/BuscarPorId/{id}", "/Api/fila/BuscarPorId/" + id);
            if (response.IsSuccessStatusCode)
                return View("ListaChamadosFila",
                    JsonConvert.DeserializeObject<FilaViewModel>(response.Content.ReadAsStringAsync().Result));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);


        }

        public IActionResult VisualizarChamado(int id)
        {
            var response = _iConsumirApi.GetMethod("/Api/Chamado/BuscarPorId/{id}", "/Api/Chamado/BuscarPorId/" + id);
            if (response.IsSuccessStatusCode)
                return View("Visualizar",
                    JsonConvert.DeserializeObject<ChamadoViewModel>(response.Content.ReadAsStringAsync().Result));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        public IActionResult AssumirChamado(int id)
        {
            return PartialView("_AssumirChamado", new AdicionarEventoViewModel() {ChamadoId = id});
        }

        [HttpPost]
        public IActionResult AssumirChamado(AdicionarEventoViewModel adicionarEventoViewModel)
        {
            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            });
            var response = _iConsumirApi.PutMethod("/Api/Chamado/AssumirChamado/{id}", "/Api/Chamado/AssumirChamado/"
                                                                                       +
                                                                                       adicionarEventoViewModel
                                                                                           .ChamadoId, json);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Home");
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);

        }
    }

}