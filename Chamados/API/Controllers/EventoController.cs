using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;
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
        
        // GET: api/Evento
        [HttpPost("Adicionar")]
        public Evento AdicionarEvento([FromBody]List<object> value)
        {
            var chamado = JsonConvert.DeserializeObject<Chamado>(value[0].ToString());
            var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());
            var evento = JsonConvert.DeserializeObject<Evento>(value[2].ToString());
            return _iEventoService.Adicionar(chamado, atendente, evento);

        }

        // GET: api/Evento/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
