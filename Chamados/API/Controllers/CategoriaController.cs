using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Categoria")]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _iCategoriaService;

        public CategoriaController(ICategoriaService iCategoriaService)
        {
            _iCategoriaService = iCategoriaService;
        }
        // GET: api/Categoria
        [HttpGet("select2/BuscarTodos")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult Get()
        {
            var item = _iCategoriaService.BuscarCategoria().Select(x => new { id = x.Codigo, text = x.Descricao, description=x.Grupo });
            
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
