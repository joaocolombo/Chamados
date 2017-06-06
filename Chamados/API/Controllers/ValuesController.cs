using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Domain.Services;
using Domain.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IChamadoService _iChamadoService;

        public ValuesController(IChamadoService iChamadoService)
        {
            _iChamadoService = iChamadoService;
        }
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return _iChamadoService.Teste();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Chamado Get(int id)
        {
            return _iChamadoService.BuscarPorId(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
