
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Services.Interfaces;
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

        public ChamadoController(IChamadoService iChamadoService, IEventoService iEventoService)
        {
            _iChamadoService = iChamadoService;
            _iEventoService = iEventoService;
        }

        //Buscar
        [HttpGet("BuscarPorAtendente/{atendente}/{finalizado}")]
        public IEnumerable<Chamado> BuscarPorAtendente(string atendente, bool finalizado)
        {
            var a = new Atendente() { Nome = atendente };
            return _iChamadoService.BuscarPorAtendente(a, finalizado);
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
                return Ok(_iChamadoService.Inserir(value));
            }
            catch (Exception ex)
            {
                return StatusCode(422, ex.Message );
            }
        }

        //Alterar OK 21/06
        [HttpPut("Finalizar/{id}")]
        public IActionResult Finalizar([FromBody]object value, int id)
        {
            try
            {
                var atendente = JsonConvert.DeserializeObject<Atendente>(value.ToString());
                _iChamadoService.Finalizar(id, atendente);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(422, ex.Message );

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

                return Ok(_iChamadoService.AlterarFilial(id, filial, atendente));
            }
            catch (Exception ex)
            {
                return StatusCode(422, ex.Message);
            }

        }

        [HttpPut("AlterarAssunto/{assunto}/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarAssunto(string assunto, string id, [FromBody]object value)
        {
            try
            {

                var atendente = JsonConvert.DeserializeObject<Atendente>(value.ToString());
                return Ok(_iChamadoService.AlterarAssunto(Convert.ToInt32(id), assunto, atendente));

            }
            catch (Exception ex)
            {
                return StatusCode(422,ex.Message );
            }


        }
        [HttpPut("AlterarSolicitante/{solicitante}/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarFilial([FromBody]object value, string solicitante ,int id)
        {
            try
            {
               
                var atendente = JsonConvert.DeserializeObject<Atendente>(value.ToString());

                return Ok(_iChamadoService.AlterarSolicitante(id, solicitante, atendente));
            }
            catch (Exception ex)
            {
                return StatusCode(422,  ex.Message );
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
                return Ok(_iChamadoService.AlterarCategoria(id, categorias, atendente));
            }
            catch (Exception ex)
            {
                return StatusCode(422, ex.Message );
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
                var evento = JsonConvert.DeserializeObject<Evento>(value[2].ToString());
               var e =_iEventoService.Adicionar(id, evento, atendente);
                _iChamadoService.AlterarFila(id, fila, atendente);
                return Ok(e);
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
                _iEventoService.AlterarAtendente(id ,atendenteNovo);
                _iChamadoService.RemoverFila(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(422, ex.Message);
            }
        }

    }
}
