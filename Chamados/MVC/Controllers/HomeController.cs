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
using MVC.Mapper;
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


        public async Task<IActionResult> Login()
        {

            var usuarioId = HttpContext.Request.Query["ucc"].ToString();
            var senha = HttpContext.Request.Query["scc"].ToString();

            if (!string.IsNullOrEmpty(usuarioId))
            {
                usuarioId = Request.Cookies[usuarioId];
                senha = Request.Cookies[senha];
            }
#if DEBUG
            //osiel
            //usuarioId = "39";
            //senha = "14EA3982F2D057A119545EE743524077";

            usuarioId = "33";
            senha = "757F66AC07CF22BD36BC793B4B2F360E";

#endif

            var retornoUsuario = _iConsumirApi.GetMethod("/api/usuario/Autenticar/{usuario}/{senha}",
          $"/api/usuario/autenticar/{usuarioId}/{senha}");

            if (retornoUsuario.IsSuccessStatusCode)
            {
                var usuario = JsonConvert.DeserializeObject<Usuario>(retornoUsuario.Content.ReadAsStringAsync().Result);

                var claims = new List<Claim>
                  {
                      new Claim(ClaimTypes.Name, usuario.NomeExibicao, ClaimValueTypes.String),
                      new Claim(ClaimTypes.NameIdentifier, usuario.Login, ClaimValueTypes.String),
                      new Claim(ClaimTypes.SerialNumber, usuario.Codigo.ToString(), ClaimValueTypes.String),
                      new Claim(ClaimTypes.Email, usuario.Email, ClaimValueTypes.String),
                  };
                foreach (var grupo in usuario.Grupos)
                    claims.Add(new Claim(ClaimTypes.Role, grupo, ClaimValueTypes.String));

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

                await HttpContext.Authentication.SignInAsync("CookieAuthentication", principal);
            }
            //var codigo = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
           var codigo = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value;
            var retorno = _iConsumirApi.GetMethod("/api/chamado/buscartodos",
                "/api/chamado/buscartodos");

            if (retorno.IsSuccessStatusCode)
                return View(JsonConvert.DeserializeObject<List<Chamado>>(retorno.Content.ReadAsStringAsync().Result));
            return StatusCode(422, retorno.Content.ReadAsStringAsync().Result);
        }

        [HttpGet]
        public IActionResult Visualizar(string id)
        {
            var response = _iConsumirApi.GetMethod("/api/chamado/buscarporid/{id}", "/api/chamado/buscarporid/" + id);
            if (response.IsSuccessStatusCode)
                return View(ChamadoTo.ChamadoViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }


        public async Task<string> AdicionarImagemAsync(IFormFile file, int id)
        {
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
                return PartialView("_Visualizar", ChamadoTo.ChamadoViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result)));
            return StatusCode(422, response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult Novo(InserirChamadoViewModel inserirChamadoViewModel)
        {
            List<Categoria> categorias = inserirChamadoViewModel.Categorias.Select(categoria => new Categoria() { Codigo = categoria }).ToList();
            var evento = new Evento(0, inserirChamadoViewModel.Descricao, new Atendente()
            {
                Codigo =Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value)
            })
            {
                MinutosPrevistos = inserirChamadoViewModel.MinutosPrevistos,
                MinutosRealizados = inserirChamadoViewModel.MinutosRealizados,
                Status = inserirChamadoViewModel.Status,
            };
            if (inserirChamadoViewModel.Geral)
                inserirChamadoViewModel.CodigoFilial = "000000";

            var chamado = new Chamado(0, "ABERTO", inserirChamadoViewModel.Assunto,
                inserirChamadoViewModel.Solicitante)
            {
                Finalizado = false,
                Filial = new Filial() { Codigo = inserirChamadoViewModel.CodigoFilial },
                Categorias = categorias,
                Eventos = new List<Evento>() { evento },
                Solicitante = inserirChamadoViewModel.Solicitante
            };

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
                return PartialView("_Visualizar", ChamadoTo.ChamadoViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result)));
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
                return PartialView("_Visualizar", ChamadoTo.ChamadoViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result)));
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
                return PartialView("_Visualizar", ChamadoTo.ChamadoViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result)));
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
