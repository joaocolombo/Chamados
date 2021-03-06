using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Services.Validates;


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
            var item = _iFilaService.BuscarFila().Select(x => new { id = x.Codigo, text = x.Descricao });

            var retorno = new
            {
                status = "success",
                itens = item,
                count = item.Count()
            };
            return Ok(retorno);
        }
        [HttpGet("BuscarTodos/Cabecarios")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult BuscarTodosCabecarios()
        {
            try
            {
                var filas = _iFilaService.BuscarFila().Select(x =>
                    new { Codigo = x.Codigo, Descricao = x.Descricao, Quantidade = x.Lista.Count });
                return Ok(filas);
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("BuscarTodos")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult BuscarTodos()
        {
            try
            {
                return Ok(_iFilaService.BuscarFila());
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpGet("BuscarPorId/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                return Ok(_iFilaService.BuscarPorId(id));
            }
            catch (RnException ex)
            {
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


    }
}