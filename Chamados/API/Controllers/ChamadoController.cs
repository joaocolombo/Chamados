
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Cors;
using Newtonsoft.Json;

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

        //Buscar
        [HttpGet("BuscarPorAtendente/{atendente}/{finalizado}")]
        public IEnumerable<Chamado> BuscarPorAtendente(string atendente, bool finalizado)
        {
            var a = new Atendente() { Nome = atendente };
            return _iChamadoService.BuscarPorAtendente(a, finalizado);
        }

        [HttpGet("BuscarPorStatus/{status}", Name = "GetPorStatus")]
        public IEnumerable<Chamado> GetPorStatus(string status)
        {
            return _iChamadoService.BuscarPorStatus(status);
        }
        [HttpGet("BuscarPorFilialCodigo/{id}")]
        public IEnumerable<Chamado> BuscarPorFilialCodigo(string id)
        {
            var filial = new Filial() { Codigo = id };
            return _iChamadoService.BuscarPorFilial(filial);
        }

        [HttpGet("BuscarPorFilialNome/{nome}")]
        public IEnumerable<Chamado> BuscarPorFilialNome(string nome)
        {
            var filial = new Filial() { Nome = nome };
            return _iChamadoService.BuscarPorFilial(filial);
        }

        [HttpGet("BuscarPorId/{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_iChamadoService.BuscarPorId(id));
        }


        //Inserir OK 21/06
        [HttpPost]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult Inserir([FromBody] Chamado value)
        {
            try
            {
                return Ok(_iChamadoService.Inserir(value));
            }
            catch (Exception ex)
            {
                return StatusCode(422, new { erro = ex.Message });
            }
        }

        //Alterar OK 21/06
        [HttpPut("Finalizar")]
        public void Finalizar([FromBody]List<object> value)
        {
            var chamado = JsonConvert.DeserializeObject<Chamado>(value[0].ToString());
            var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());
            _iChamadoService.Finalizar(chamado, atendente);
        }

        [HttpPut("AlterarFilial/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarFilial([FromBody]List<object> value, int id)
        {
            try
            {
                var filial = JsonConvert.DeserializeObject<Filial>(value[0].ToString());
                var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());

                return Ok(_iChamadoService.AlterarFilial(id, filial, atendente));
            }
            catch (Exception ex)
            {
                return StatusCode(422, new{erro=ex.Message});
            }

        }

        [HttpPut("AlterarAssunto/{assunto}/{id}")]
        [EnableCors("LiberarAcessoExterno")]
        public IActionResult AlterarAssunto(string assunto, string id, [FromBody]object value)
        {
            try
            {

                var atendente = JsonConvert.DeserializeObject<Atendente>(value.ToString());
                return Ok(_iChamadoService.AlterarAssunto(Convert.ToInt32(id), assunto, atendente));

            }
            catch (Exception ex)
            {
               return StatusCode(422, new { ex.Message});
            }


        }

        [HttpPut("AlterarCategoria/{id}")]
        [EnableCors("LiberarAcessoExterno")]

        public Chamado AlterarCategoria([FromBody]List<object> value, int id)
        {
           
            var categorias = JsonConvert.DeserializeObject<List<Categoria>>(value[0].ToString());
            var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());
            return _iChamadoService.AlterarCategoria(id, categorias, atendente);
        }

    }
}
