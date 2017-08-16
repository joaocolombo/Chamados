using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using MVC.Interfaces;
using MVC.ViewModel.Evento;
using MVC.ViewModel.Home;

namespace MVC.Controllers
{

    public class HomeController : Controller
    {
        private readonly IConsumirApi _iConsumirApi;

        public HomeController(IConsumirApi iConsumirApi)
        {
            _iConsumirApi = iConsumirApi;
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return PartialView("_Novo");
        }

        [HttpGet]
        public IActionResult AlterarAssunto(string id, string assunto)
        {
            return PartialView("_AlterarAssunto", new AlterarAssuntoViewModel() { Assunto = assunto, Id = id });
        }

        [HttpGet]
        public IActionResult AlterarFilial(string id, string filial)
        {
            return PartialView("_AlterarFilial", new AlterarFilialViewModel() { Id = id, Filial = filial });
        }

        [HttpGet]
        public IActionResult AlterarCategoria(string id, IEnumerable<int> categorias)
        {
            return PartialView("_AlterarCategoria", new AlterarCategoriaViewModel() { Id = id, Categorias = categorias });
        }

        [HttpGet]
        public IActionResult AlterarSolicitante(string id, string solicitante)
        {
            return PartialView("_AlterarSolicitante",
                new AlterarSolicitanteViewModel() { Id = id, Solicitante = solicitante });
        }

        [HttpGet]
        public IActionResult Finalizar(string id, string nomeAtendente)
        {
            return PartialView("_Finalizar", new FinalizarViewModel() { Id = id, NomeAtendente = nomeAtendente });
        }

        //[Route("")]
        //[Route("/{usuario?}/{senha?}")]
        //public IActionResult Index(string usuarioId, string senha){
        //if (!string.IsNullOrEmpty(usuario))
        //{
        //    usuario = Request.Cookies[usuario];
        //    senha = Request.Cookies[senha];
        //}
        [HttpGet]
        public IActionResult Index()
        {

            //  var retornoUsuario = _iConsumirApi.GetMethod("/api/usuario/Autenticar/{usuario}/{senha}",
            //"/api/usuario/autenticar/33/757F66AC07CF22BD36BC793B4B2F360E");

            //  if (retornoUsuario.IsSuccessStatusCode)
            //  {
            //      var usuario = JsonConvert.DeserializeObject<Usuario>(retornoUsuario.Content.ReadAsStringAsync().Result);
            //      var claims = new List<Claim>
            //      {
            //          new Claim(ClaimTypes.Name, usuario.NomeExibicao, ClaimValueTypes.String),
            //          new Claim(ClaimTypes.NameIdentifier, usuario.Login, ClaimValueTypes.String),
            //          new Claim(ClaimTypes.SerialNumber, usuario.Id.ToString(), ClaimValueTypes.String),
            //          new Claim(ClaimTypes.Email, usuario.Email, ClaimValueTypes.String),
            //      };
            //      foreach (var grupo in usuario.Grupos)
            //          claims.Add(new Claim(ClaimTypes.Role, grupo, ClaimValueTypes.String));

            //      ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            //      HttpContext.Authentication.SignInAsync("CookieAuthentication", principal);
            //  }
            //  var atendenteId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value;

            var retoro = _iConsumirApi.GetMethod("/api/chamado/buscarporatendente",
                "/api/chamado/buscarporatendente/" + 33 + "/1");
            if (retoro.IsSuccessStatusCode)
                return View(JsonConvert.DeserializeObject<List<Chamado>>(retoro.Content.ReadAsStringAsync().Result));
            return StatusCode(422, retoro.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        public IActionResult Visualizar(string id)
        {
            var response = _iConsumirApi.GetMethod("/api/chamado/buscarporid/{id}", "/api/chamado/buscarporid/" + id);
            if (response.IsSuccessStatusCode)
                return View(ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }

        private ChamadoViewModel ConvertChamadoToViewModel(Chamado chamado)
        {
            List<AdicionarEventoViewModel> eventos = new List<AdicionarEventoViewModel>();
            foreach (var evento in chamado.Eventos)
            {
                eventos.Add(new AdicionarEventoViewModel()
                {
                    Descricao = evento.Descricao,
                    Status = evento.Status,
                    Abertura = evento.Abertura,
                    Atendente = evento.Atendente,
                    Codigo = evento.Codigo,
                    ChamadoId = chamado.Codigo,
                    FilaId = evento.Codigo,
                    MinutosPrevistos = evento.MinutosPrevistos,
                    MinutosRealizados = evento.MinutosRealizados
                });
            }
            return new ChamadoViewModel()
            {
                Assunto = chamado.Assunto,
                Codigo = chamado.Codigo,
                Categorias = chamado.Categorias,
                Filial = chamado.Filial,
                Finalizado = chamado.Finalizado,
                Solicitante = chamado.Solicitante,
                Status = chamado.Status,
                Eventos = eventos
            };
        }
        public async Task<string> AdicionarImagemAsync(IFormFile file, int id)
        {
            var ida = ViewBag.Id;
            var nomeArquivo = Guid.NewGuid() + file.FileName;
            if (file.Length > 0)
            {
                using (var stream = new FileStream("c:\\temp\\" + nomeArquivo, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            });

            var response = _iConsumirApi.PutMethod("/api/Chamado/AdicionarImagem/{nomeArquivo}/{id}", "/api/Chamado/AdicionarImagem/" + nomeArquivo + "/" + id, json);
            if (response.IsSuccessStatusCode)
                return "sucesso";
            return "Erro ao Inserir";
        }

        [HttpPost]
        public IActionResult AlterarSolicitante(AlterarSolicitanteViewModel alterarSolicitante)
        {
            if (string.IsNullOrEmpty(alterarSolicitante.Solicitante))
                return StatusCode(422, "Prencha o Solicitante");
            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            });
            var response = _iConsumirApi.PutMethod("/api/Chamado/AlterarSolicitante/{assunto}/{id}",
                "/api/Chamado/AlterarSolicitante/" + alterarSolicitante.Solicitante + "/" + alterarSolicitante.Id,
                json);
            if (response.IsSuccessStatusCode)
                return PartialView("_Visualizar", ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult Novo(InserirChamadoViewModel inserirChamadoViewModel)
        {
            List<Categoria> categorias = inserirChamadoViewModel.Categorias.Select(categoria => new Categoria() { Codigo = categoria }).ToList();
            var evento = new Evento(0, inserirChamadoViewModel.Descricao, new Atendente(){Codigo = 33})
            {
                MinutosPrevistos = inserirChamadoViewModel.MinutosPrevistos,
                MinutosRealizados = inserirChamadoViewModel.MinutosRealizados,
            };
            evento.SetStatus(inserirChamadoViewModel.Status);
            if (inserirChamadoViewModel.Geral)
                inserirChamadoViewModel.CodigoFilial = "000000";

            var chamado = new Chamado(0, "ABERTO", inserirChamadoViewModel.Assunto,
                inserirChamadoViewModel.Solicitante)
            { Finalizado = false };

            chamado.SetFilial(new Filial() { Codigo = inserirChamadoViewModel.CodigoFilial });
            chamado.SetCategoria(categorias);
            chamado.SetEventos(new List<Evento>() { evento });
            chamado.SetSolicitante(inserirChamadoViewModel.Solicitante);

            var response = _iConsumirApi.PostMethod("/api/Chamado", JsonConvert.SerializeObject(chamado));
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Visualizar", new { id = response.Content.ReadAsStringAsync().Result });
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult Finalizar(FinalizarViewModel finalizar)
        {
            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            });
            var response = _iConsumirApi.PutMethod("/api/Chamado/Finalizar/{id}", "/api/Chamado/Finalizar/" + finalizar.Id, json);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult AlterarCategoria(AlterarCategoriaViewModel alterarCategoria)
        {
            List<Categoria> listaCategoria = new List<Categoria>();
            var atendente = new Atendente()
            {
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            };
            if (alterarCategoria.Categorias != null)
                foreach (var codigo in alterarCategoria.Categorias)
                    listaCategoria.Add(new Categoria() { Codigo = codigo });

            List<object> lista = new List<object>() { listaCategoria, atendente };
            var response = _iConsumirApi.PutMethod("/api/Chamado/AlterarCategoria/{id}",
                "/api/Chamado/AlterarCategoria/" + alterarCategoria.Id, JsonConvert.SerializeObject(lista));
            if (response.IsSuccessStatusCode)
                return PartialView("_Visualizar", ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult AlterarAssunto(AlterarAssuntoViewModel alterarAssunto)
        {
            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            });
            if (string.IsNullOrEmpty(alterarAssunto.Assunto))
                return StatusCode(422, "Prencha o Assunto");

            var response = _iConsumirApi.PutMethod("/api/Chamado/AlterarAssunto/{assunto}/{id}", "/api/Chamado/AlterarAssunto/" + alterarAssunto.Assunto + "/" + alterarAssunto.Id, json);
            if (response.IsSuccessStatusCode)
                return PartialView("_Visualizar", ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }


        [HttpPost]
        public IActionResult AlterarFilial(AlterarFilialViewModel alterarFilial)
        {
            var atendente = new Atendente()
            {
                Codigo = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            };
            var filial = new Filial() { Codigo = alterarFilial.Filial };
            List<object> lista = new List<object>() { filial, atendente };
            var json = JsonConvert.SerializeObject(lista);
            var response = _iConsumirApi.PutMethod("/api/Chamado/AlterarFilial/{id}",
                    "/api/Chamado/AlterarFilial/" + alterarFilial.Id, json);
            if (response.IsSuccessStatusCode)
                return PartialView("_Visualizar", ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);


        }

        //---------------------------------
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

        public IActionResult Error(string error)
        {
            ViewData["Error"] = error;
            return View();
        }
    }
}
