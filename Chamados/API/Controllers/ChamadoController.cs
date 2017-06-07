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

        //GET: api/Chamado/BuscarPorStatus/aberto
        [HttpGet("BuscarPorStatus/{status}", Name="GetPorStatus")]
        public IEnumerable<Chamado> GetPorStatus(string status)
        {
            return _iChamadoService.BuscarPorStatus(status);
        }

        // GET: api/Chamado/BuscarPorId/5
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
        
        // PUT: api/Chamado/Encerrar/5
        [HttpPut("Encerrar/{id}")]
        public void PutFinalizar(int id, [FromBody]Chamado value)
        {
            var x = id;
            var corpo = value;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
