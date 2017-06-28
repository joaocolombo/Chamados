using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.ViewModel;

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
                var stringData = response.Content.ReadAsStringAsync().Result;
                return View(JsonConvert.DeserializeObject<List<Chamado>>(stringData));
            }
        }
        [HttpGet]
        public IActionResult Novo()
        {
            return PartialView("_Novo");

        }
        [HttpPost]
        public IActionResult Novo(InserirChamadoViewModel inserirChamadoViewModel)
        {
            List<Categoria> categorias = inserirChamadoViewModel.Categorias.Select(categoria => new Categoria() { Codigo = categoria }).ToList();
            var evento = new Evento()
            {
                Descricao = inserirChamadoViewModel.Descricao,
                Atendente = new Atendente()
                {
                    Nome = inserirChamadoViewModel.NomeAtendente
                },
                Status = inserirChamadoViewModel.Status
                
            };
            if (inserirChamadoViewModel.Geral)
            {
                inserirChamadoViewModel.CodigoFilial = "000000";
            }
            
            var chamado = new Chamado()
            {
                Assunto = inserirChamadoViewModel.Assunto,
                Status ="ABERTO",
                Filial = new Filial() { Codigo = inserirChamadoViewModel.CodigoFilial },
                Finalizado = false,
                Categorias = categorias,
                Eventos = new List<Evento>() { evento }
            };

            var json = JsonConvert.SerializeObject(chamado);
            using (var client = new HttpClient())
            {
                //client.PostAsync(url+ "/api/Chamado", new StringContent(json, Encoding.UTF8, "application/json"));

                client.BaseAddress = new Uri(url + "/api/Chamado");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.PostAsync(url + "/api/Chamado", new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;
                var stringData = response.Content.ReadAsStringAsync().Result;
                return View("About");

            }

 
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
