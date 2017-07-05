using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;


namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Fila")]
    public class FilaController : Controller
    {

        private readonly IFilaService _iFilaService;

        public FilaController(IFilaService iFilaService)
        {
            _iFilaService = iFilaService;
        }



        // GET: api/Fila
        [HttpGet("select2/BuscarTodos")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult Get()
        {
            var item = _iFilaService.BuscarFila().Select(x => new {id = x.Codigo, text = x.Descricao});

            var retorno = new
            {
                status = "success",
                itens = item,
                count = item.Count()
            };
            return Ok(retorno);
        }



    }
}