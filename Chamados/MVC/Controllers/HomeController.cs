using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.ViewModel;
using MVC.ViewModel.Evento;
using MVC.ViewModel.Home;

namespace MVC.Controllers
{

    public class HomeController : Controller
    {
        private string url = "http://10.1.0.4";


        private 



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



        [HttpGet]
        //[Route("")]
        //[Route("/{usuario?}/{senha?}")]
        //public IActionResult Index(string usuario, string senha)
        public IActionResult Index()
        {
            //if (!string.IsNullOrEmpty(usuario))
            //{
            //    usuario = Request.Cookies[usuario];
            //    senha = Request.Cookies[senha];
            //}




            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Joao", ClaimValueTypes.String),
                new Claim(ClaimTypes.Country, "UK", ClaimValueTypes.String),
                new Claim("ChildhoodHero", "Ronnie James Dio", ClaimValueTypes.String),
                new Claim(ClaimTypes.Email, "joao.co@hotmail.com", ClaimValueTypes.String),
                new Claim(ClaimTypes.Role, "pinto", ClaimValueTypes.String)
            };

            var userIdentity = new ClaimsIdentity(claims, "CookieAutentication");

            var userPrincipal = new ClaimsPrincipal(userIdentity);

            HttpContext.Authentication.SignInAsync("CookieAutentication", userPrincipal,
                 new AuthenticationProperties
                 {
                     ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                     IsPersistent = false,
                     AllowRefresh = false
                 });




#if DEBUG
            CookieOptions cookies = new CookieOptions();
            cookies.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("3B0A953170186B25414F47C59F15137B", "33");

            //3B0A953170186B25414F47C59F15137B
#endif

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url + "/api/chamado/buscarporatendente");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response = client.GetAsync("/api/chamado/buscarporatendente/" + 33 + "/1").Result;
                var stringData = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    return View(JsonConvert.DeserializeObject<List<Chamado>>(stringData));
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return Error(response.Content.ReadAsStringAsync().Result);
            }
        }
        [HttpGet]
        [Authorize(Roles = "pinto")]
        public IActionResult Visualizar(string id)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url + "/api/chamado/buscarporid/{id}");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response = client.GetAsync("/api/chamado/buscarporid/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    var stringData = response.Content.ReadAsStringAsync().Result;

                    var chamadoVM = ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(stringData));
                    return View(chamadoVM);
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return Error(response.Content.ReadAsStringAsync().Result);

            }

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
                    FilaId = evento.Codigo

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


        public async Task<IActionResult> AdicionarImagemAsync(IFormFile file, int id)
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
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
            });

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + "/api/Chamado/AdicionarImagem/{nomeArquivo}/{id}");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.PutAsync(url + "/api/Chamado/AdicionarImagem/" + nomeArquivo + "/" + id, new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;
                if (response.IsSuccessStatusCode)
                {
                    var chamadoVM = ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result));
                    return PartialView("_Visualizar", chamadoVM);
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return Error(response.Content.ReadAsStringAsync().Result);

            }

        }

        [HttpPost]
        public IActionResult AlterarSolicitante(AlterarSolicitanteViewModel alterarSolicitante)
        {


            if (string.IsNullOrEmpty(alterarSolicitante.Solicitante))
            {
                return StatusCode(422, "Prencha o Solicitante");
            }

            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
            });


            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + "/api/Chamado/AlterarSolicitante/{assunto}/{id}");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.PutAsync(url + "/api/Chamado/AlterarSolicitante/" + alterarSolicitante.Solicitante + "/" + alterarSolicitante.Id, new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;
                if (response.IsSuccessStatusCode)
                {
                    var chamadoVM = ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result));
                    return PartialView("_Visualizar", chamadoVM);
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return Error(response.Content.ReadAsStringAsync().Result);

            }
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
                    Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
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
                Status = "ABERTO",
                Filial = new Filial() { Codigo = inserirChamadoViewModel.CodigoFilial },
                Finalizado = false,
                Categorias = categorias,
                Eventos = new List<Evento>() { evento },
                Solicitante = inserirChamadoViewModel.Solicitante
            };

            var json = JsonConvert.SerializeObject(chamado);
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + "/api/Chamado");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.PostAsync(url + "/api/Chamado", new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;
                var id = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Visualizar", new { id = id });
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return Error(response.Content.ReadAsStringAsync().Result);

            }

        }

        [HttpPost]
        public IActionResult Finalizar(FinalizarViewModel finalizar)
        {
            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])


            });
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + "/api/Chamado/Finalizar/{id}");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.PutAsync(url + "/api/Chamado/Finalizar/" + finalizar.Id,
                            new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;
                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return Error(response.Content.ReadAsStringAsync().Result);

            }
        }

        [HttpPost]
        public IActionResult AlterarCategoria(AlterarCategoriaViewModel alterarCategoria)
        {
            List<Categoria> listaCategoria = new List<Categoria>();

            var atendente = new Atendente()
            {
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])


            };
            if (alterarCategoria.Categorias != null)
            {

                foreach (var codigo in alterarCategoria.Categorias)
                {
                    listaCategoria.Add(new Categoria() { Codigo = codigo });
                }

            }

            List<object> lista = new List<object>();
            lista.Add(listaCategoria);
            lista.Add(atendente);
            var json = JsonConvert.SerializeObject(lista);

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + "/api/Chamado/AlterarCategoria/{id}");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.PutAsync(url + "/api/Chamado/AlterarCategoria/" + alterarCategoria.Id, new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;
                if (response.IsSuccessStatusCode)
                {
                    var chamadoVM = ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result));
                    return PartialView("_Visualizar", chamadoVM);
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return Error(response.Content.ReadAsStringAsync().Result);

            }
        }


        [HttpPost]
        public IActionResult AlterarAssunto(AlterarAssuntoViewModel alterarAssunto)
        {
            var json = JsonConvert.SerializeObject(new Atendente()
            {
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])


            });
            if (string.IsNullOrEmpty(alterarAssunto.Assunto))
            {
                return StatusCode(422, "Prencha o Assunto");
            }

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url + "/api/Chamado/AlterarAssunto/{assunto}/{id}");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.PutAsync(url + "/api/Chamado/AlterarAssunto/" + alterarAssunto.Assunto + "/" + alterarAssunto.Id, new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;
                if (response.IsSuccessStatusCode)
                {
                    var chamadoVM = ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result));
                    return PartialView("_Visualizar", chamadoVM);
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return Error(response.Content.ReadAsStringAsync().Result);

            }
        }


        [HttpPost]
        public IActionResult AlterarFilial(AlterarFilialViewModel alterarFilial)
        {
            var atendente = new Atendente()
            {
                Codigo = Convert.ToInt32(Request.Cookies["3B0A953170186B25414F47C59F15137B"])
            };
            var filial = new Filial() { Codigo = alterarFilial.Filial };
            List<object> lista = new List<object>();
            lista.Add(filial);
            lista.Add(atendente);
            var json = JsonConvert.SerializeObject(lista);

            using (var client = new HttpClient())
            {
                //client.PostAsync(url+ "/api/Chamado", new StringContent(json, Encoding.UTF8, "application/json"));

                client.BaseAddress = new Uri(url + "/api/Chamado/AlterarFilial/{id}");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response =
                    client.PutAsync(url + "/api/Chamado/AlterarFilial/" + alterarFilial.Id, new StringContent(json, Encoding.UTF8, "application/json"))
                        .Result;
                if (response.IsSuccessStatusCode)
                {
                    var chamadoVM = ConvertChamadoToViewModel(JsonConvert.DeserializeObject<Chamado>(response.Content.ReadAsStringAsync().Result));
                    return PartialView("_Visualizar", chamadoVM);
                }
                if (response.ReasonPhrase.Equals("Unprocessable Entity"))
                {
                    return StatusCode(422, response.Content.ReadAsStringAsync().Result);
                }
                return Error(response.Content.ReadAsStringAsync().Result);

            }

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
