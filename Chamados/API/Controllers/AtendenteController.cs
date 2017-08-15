using System;
using System.Linq;
using Domain.Entities;
using Domain.Services.Interfaces;
using Domain.Services.Validates;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Atendente")]
    public class AtendenteController : Controller
    {
        private readonly IAtendenteService _iAtendenteService;

        public AtendenteController(IAtendenteService iAtendenteService)
        {
            _iAtendenteService = iAtendenteService;
        }


        [HttpGet("select2/BuscarTodos")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult Get()
        {
            var item = _iAtendenteService.BuscarTodosAtendete().Select(x => new { id = x.Nome, text = x.NomeExibicao, description = x.Nivel });

            var retorno = new
            {
                status = "success",
                itens = item,
                count = item.Count()
            };
            return Ok(retorno);
        }

        [HttpGet("BuscarPorNome/{nome}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult BuscarPorNome(string nome)
        {
            try
            {
                return Ok(_iAtendenteService.BuscarAtendente(nome));
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
        public IActionResult BuscarPorNome(int id)
        {
            try
            {
                return Ok(_iAtendenteService.BuscarAtendente(id));
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