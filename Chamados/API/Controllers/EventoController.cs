using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Evento")]
    public class EventoController : Controller
    {

        private readonly IEventoService _iEventoService;

        public EventoController(IEventoService iEventoService)
        {
            _iEventoService = iEventoService;
        }


        [HttpPost("Adicionar")]
        public Evento AdicionarEvento([FromBody]List<object> value)
        {
            var chamado = JsonConvert.DeserializeObject<Chamado>(value[0].ToString());
            var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());
            var evento = JsonConvert.DeserializeObject<Evento>(value[2].ToString());
            return _iEventoService.Adicionar(chamado, evento, atendente);

        }

        [HttpGet("Select2/BuscarStatus")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult BuscarStatus()
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

        [HttpPut("AlterarDescricao/{descricao}")]
        public Evento AlterarDescricao([FromBody] List<object> value, string descricao)
        {
            var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());
            var evento = JsonConvert.DeserializeObject<Evento>(value[0].ToString());
            return _iEventoService.AlterarDescricao(evento, descricao, atendente);
        }

        // GET: api/Evento/5
        [HttpGet("BuscarPorId/{id}")]
        public Evento Get(int id)
        {
            return _iEventoService.BuscarPorId(id);
        }

        // POST: api/Evento
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Evento/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
