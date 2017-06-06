using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Services.Interfaces;
using System.Runtime.Serialization.Json;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Chamado")]

    public class ChamadoController : Controller
    {
        private readonly IChamadoService _iChamadoService;

        public ChamadoController(IChamadoService iChamadoService)
        {
            _iChamadoService = iChamadoService;
        }

        //GET: api/Chamado
        [HttpGet("BuscarPorStatus/{status}", Name="GetPorStatus")]
        public string GetPorStatus(string status)
        {
            return "acerto miseravi"+status;
        }

        // GET: api/Chamado/5
        [HttpGet("BuscarPorId/{id}", Name = "Get")]
        public Chamado Get(int id)
        {
            return _iChamadoService.BuscarPorId(id);
        }
        
        // POST: api/Chamado
        [HttpPost]
        public void Post([FromBody]Chamado value)
        {

            var x = value;

        }
        
        // PUT: api/Chamado/5
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
