using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Services.Interfaces;
using System.Runtime.Serialization.Json;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils.Messaging;
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
        [HttpGet("BuscarPorAtendente/{atendente}/{status}")]
        public IEnumerable<Chamado> BuscarPorAtendente(string nome, string status)
        {
            var atendente = new Atendente() {Nome = nome};
            return _iChamadoService.BuscarPorAtendente(atendente,status);
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

        [HttpGet("BuscarPorId/{id}", Name = "Get")]
        public Chamado Get(int id)
        {
            return _iChamadoService.BuscarPorId(id);
        }
        //Inserir
        [HttpPost]
        public int Inserir([FromBody]Chamado value)
        {
            return _iChamadoService.Inserir(value);
        }
        //Alterar
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

        //void EncaminharN2(Evento evento, Chamado chamado);
        //void Encaminhar(Evento evento, Atendente atendente, Chamado chamado);

    }
}
