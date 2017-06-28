
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
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
            var a = new Atendente() {Nome = atendente };
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
        public Chamado Get(int id)
        {
            return _iChamadoService.BuscarPorId(id);
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
                return BadRequest(ex.Message);
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

        [HttpPut("AlterarFilial")]
        public Chamado AlterarFilial([FromBody]List<object> value)
        {
            var chamdo = JsonConvert.DeserializeObject<Chamado>(value[0].ToString());
            var filial = JsonConvert.DeserializeObject<Filial>(value[1].ToString());
            var atendente = JsonConvert.DeserializeObject<Atendente>(value[2].ToString());

            return _iChamadoService.AlterarFilial(chamdo, filial, atendente);
        }

        [HttpPut("AlterarAssunto/{assunto}")]
        public Chamado AlterarAssunto(string assunto, [FromBody]List<object> value)
        {
            var chamado = JsonConvert.DeserializeObject<Chamado>(value[0].ToString());
            var atendente = JsonConvert.DeserializeObject<Atendente>(value[1].ToString());

            return _iChamadoService.AlterarAssunto(chamado, assunto, atendente);
        }

        [HttpPut("AlterarCategoria")]
        public Chamado AlterarCategoria([FromBody]List<object> value)
        {
            var chamado = JsonConvert.DeserializeObject<Chamado>(value[0].ToString());
            var categorias = JsonConvert.DeserializeObject<List<Categoria>>(value[1].ToString());
            var atendente = JsonConvert.DeserializeObject<Atendente>(value[2].ToString());
            return _iChamadoService.AlterarCategoria(chamado, categorias, atendente);
        }

    }
}
