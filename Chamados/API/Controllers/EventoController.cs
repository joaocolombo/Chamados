using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Evento")]
    public class EventoController : Controller
    {

        private readonly IEventoService _iEventoService;
        private readonly IAtendenteService _iAtendenteService;

        public EventoController(IEventoService iEventoService, IAtendenteService iAtendenteService)
        {
            _iEventoService = iEventoService;
            _iAtendenteService = iAtendenteService;
        }


        [HttpPost("Adicionar")]
        [EnableCors("LiberarAcessoExterno")]

        public IActionResult AdicionarEvento([FromBody]List<object> value)
        {
            try
            {
                var chamado = Convert.ToInt32(value[0]);
                var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);
                var evento = JsonConvert.DeserializeObject<Evento>(value[2].ToString());
                evento.Atendente= _iAtendenteService.BuscarAtendente(evento.Atendente.Codigo);
                return Ok(_iEventoService.Adicionar(chamado, evento, atendente));
            }
            catch (Exception ex)
            {
                return StatusCode(422, ex.Message );
            }

        }

        [HttpGet("Select2/BuscarStatus")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult BuscarStatus()
        {
            try
            {

                var item = _iEventoService.BuscarStatus();
                var retorno = new
                {
                    status = "success",
                    itens = item,
                    count = item.Count()
                };
                return Ok(retorno);

            }
            catch (Exception ex)
            {
                return StatusCode(422, ex.Message );
            }
        }

        [HttpPut("AlterarDescricao/{descricao}/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarDescricao([FromBody] object value, string descricao, int id)
        {
            try
            {
                var atendente = JsonConvert.DeserializeObject<Atendente>(value.ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);

                return Ok(_iEventoService.AlterarDescricao(id, descricao, atendente));
            }
            catch (Exception ex)
            {
                return StatusCode(422,ex.Message );
            }
        }

        [HttpPut("AlterarStatus/{status}/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarStatus([FromBody] object value, string status, int id)
        {
            try
            {
                var atendente = JsonConvert.DeserializeObject<Atendente>(value.ToString());
                atendente = _iAtendenteService.BuscarAtendente(atendente.Codigo);

                return Ok(_iEventoService.AlterarStatus(id, status, atendente));
            }
            catch (Exception ex)
            {
                return StatusCode(422,  ex.Message );
            }
        }
        // GET: api/Evento/5
        [HttpGet("BuscarPorId/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_iEventoService.BuscarPorId(id));
            }
            catch (Exception ex)
            {
                return StatusCode(422,ex.Message );
            }
        }


    }
}
