
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Services.Interfaces;
using Domain.Services.Validates;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Chamado")]

    public class ChamadoController : Controller
    {
        private readonly IChamadoService _iChamadoService;
        private readonly IEventoService _iEventoService;
        private readonly IAtendenteService _iAtendenteService;

        public ChamadoController(IChamadoService iChamadoService, IEventoService iEventoService, IAtendenteService iAtendenteService)
        {
            _iChamadoService = iChamadoService;
            _iEventoService = iEventoService;
            _iAtendenteService = iAtendenteService;
        }

        //Buscar
        [HttpGet("BuscarPorAtendente/{atendente}/{finalizado}")]
        public IEnumerable<Chamado> BuscarPorAtendente(int atendente, bool finalizado)
        {

            var a = _iAtendenteService.BuscarAtendente(atendente);
            return _iChamadoService.BuscarPorAtendente(a, finalizado);
        }

        [HttpGet("BuscarTodos")]
        public IEnumerable<Chamado> BuscarTodos()
        {

           return _iChamadoService.BuscarTodos();
        }

        [HttpGet("BuscarPorStatus/{status}", Name = "GetPorStatus")]
        public IEnumerable<Chamado> GetPorStatus(string status)
        {
            return _iChamadoService.BuscarPorStatus(status);
        }
        [HttpGet("BuscarPorFilialCodigo/{id}")]
        public IEnumerable<Chamado> BuscarPorFilialCodigo(string id)
        {
            var filial = new Filial() { Codigo = id };
            return _iChamadoService.BuscarPorFilial(filial);
        }

        [HttpGet("BuscarPorFilialNome/{nome}")]
        public IEnumerable<Chamado> BuscarPorFilialNome(string nome)
        {
            var filial = new Filial() { Nome = nome };
            return _iChamadoService.BuscarPorFilial(filial);
        }

        [HttpGet("BuscarPorId/{id}")]
        public IActionResult Get(int id)
        {

            return Ok(_iChamadoService.BuscarPorId(id));
        }


        //Inserir OK 21/06
        [HttpPost]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult Inserir([FromBody] Chamado value)
        {
            try
            {
                foreach (var evento in value.Eventos)
                {
                    evento.Atendente = _iAtendenteService.BuscarAtendente(evento.Atendente.Codigo);
                }
                return Ok(_iChamadoService.Inserir(value));
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Alterar OK 21/06
        [HttpPut("Finalizar/{id}")]
        public IActionResult Finalizar([FromBody]object value, int id)
        {
            try
            {
                var atendente = JsonConvert.DeserializeObject<Atendente>(value.ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);
                _iChamadoService.Finalizar(id, atendente);
                return Ok();
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }

        }

        [HttpPut("AlterarFilial/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarFilial([FromBody]List<object> value, int id)
        {
            try
            {

                var filial = JsonConvert.DeserializeObject<Filial>(value[0].ToString());
                var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);

                return Ok(_iChamadoService.AlterarFilial(id, filial, atendente));
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("AlterarAssunto/{assunto}/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarAssunto(string assunto, string id, [FromBody]object value)
        {
            try
            {
                var atendente = JsonConvert.DeserializeObject<Atendente>(value.ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);
                return Ok(_iChamadoService.AlterarAssunto(Convert.ToInt32(id), assunto, atendente));
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }
        [HttpPut("AlterarSolicitante/{solicitante}/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarSolicitante([FromBody]object value, string solicitante, int id)
        {
            try
            {

                var atendente = JsonConvert.DeserializeObject<Atendente>(value.ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);

                return Ok(_iChamadoService.AlterarSolicitante(id, solicitante, atendente));
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("AlterarCategoria/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarCategoria([FromBody]List<object> value, int id)
        {
            try
            {
                var categorias = JsonConvert.DeserializeObject<List<Categoria>>(value[0].ToString());
                var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);

                return Ok(_iChamadoService.AlterarCategoria(id, categorias, atendente));
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("AlterarFila/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarFila([FromBody]List<object> value, int id)
        {
            try
            {

                var fila = JsonConvert.DeserializeObject<Fila>(value[0].ToString());
                var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);
                _iChamadoService.AlterarFila(id, fila, atendente);

                var evento = JsonConvert.DeserializeObject<Evento>(value[2].ToString());
                var e = _iEventoService.AdicionarEventoFila(id, evento);

                return Ok(e);
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("Encaminhar/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult EncaminharOutroAtendente([FromBody]List<object> value, int id)
        {
            try
            {
                //atendente atual
                var atendente = JsonConvert.DeserializeObject<Atendente>(value[0].ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);
                //novo atendente
                var evento = JsonConvert.DeserializeObject<Evento>(value[1].ToString());
                evento.Atendente = _iAtendenteService.BuscarAtendente(evento.Atendente.Nome);
                var e = _iEventoService.AdicionarEventoOutroAtendente(id, evento, atendente);

                return Ok(e);
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("AdicionarImagem/{nomeArquivo}/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AdicionaImagem([FromBody] object value, string nomeArquivo, int id)
        {
            try
            {
                var atendente = JsonConvert.DeserializeObject<Atendente>(value.ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);

                return Ok(_iChamadoService.AdicionarImagem(id, nomeArquivo, atendente));
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(422, ex.Message);
            }
        }


        [HttpPut("AssumirChamado/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AssumirChamado([FromBody]object value, int id)
        {
            try
            {

                var atendenteNovo = JsonConvert.DeserializeObject<Atendente>(value.ToString());
                atendenteNovo = _iAtendenteService.BuscarAtendente(atendenteNovo.Codigo);

                _iEventoService.AssumirChamado(id, atendenteNovo);
                _iChamadoService.RemoverFila(id);
                return Ok();
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //[HttpGet("Tabela/{tabela}/{draw}/{start}/{length}/{sortColumn}/{sortColumnDir}/{searchFilter}")]
        //string draw, string start, string length, string sortColumn, string sortColumnDir, string searchFilter
        [EnableCors("LiberarAcessoExterno")]
        [HttpPost("Tabela")]
        public IActionResult TabelaChamados()
        {

            var teste = Request.Form;

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();
            var sortColumn =
                HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() +
                                       "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchFilter = Request.Form["search[value]"].FirstOrDefault();

            var tabela = "chamado";

            //var draw = values("draw").FirstOrDefault(); //qtd linhas da tabela
            //var start = Request.Form.GetValues("start").FirstOrDefault(); //primeira linha
            //var length = Request.Form.GetValues("length").FirstOrDefault();//qtd total de registros
            //var sortColumn =Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() +
            //                         "][name]").FirstOrDefault();//coluna que sera ordenada
            // var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();//forma de ordena��o desc 
            // var searchFilter = Request.Form.GetValues("search[value]").FirstOrDefault();// parametro filtro

            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;


            var data = _iChamadoService.SelectGenerico(tabela, searchFilter, draw, sortColumn, sortColumnDir, start, length);

            var recordsTotal = _iChamadoService.TotalRegistros(tabela, searchFilter);//total de linas na tabela 

            return Json(new
            {
                draw,
                recordsTotal,
                recordsFiltered = data.Count(),
                data = data
            });
        }

        [HttpGet("Tabela2")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult TabelaChamadosa()
        {
            List<object> data = new List<object>();
            var obj1 = new
            { name = "Tiger Nixon",
                position = "System Architect",
                salary = "$3,120",
                start_date = "2011/04/25",
                office = "Edinburgh",
                extn = "5421"
            };
            for (int i = 0; i < 30; i++)
            {
                data.Add(obj1);
            }

            return Ok(data);
        }

    }
}
