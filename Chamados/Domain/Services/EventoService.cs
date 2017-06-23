using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _iEventoRepository;
        private readonly IChamadoService _iChamadoService;

        public EventoService(IEventoRepository iEventoRepository, IChamadoService iChamadoService)
        {
            _iEventoRepository = iEventoRepository;
            _iChamadoService = iChamadoService;
        }

        private void ValidarAtendenteCorrente(Atendente atendente, Evento evento)
        {
            if (atendente.Nome != evento.Atendente.Nome)
            {
                throw new Exception("O Evento não pode ser adicionado/alterado por esse atendente");
            }
        }
        
        public Evento Adicionar(Chamado chamado, Evento evento, Atendente atendente)
        {
            var erro = "";
            ValidarAtendenteCorrente(atendente, evento);
            if (string.IsNullOrEmpty(evento.Descricao))
            {
                erro = "Necessario preencher uma descricao";
            }
            if (string.IsNullOrEmpty(evento.Status))
            {
                erro += "Necessario selecionar um status";
            }
            if (!string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }
            var c = _iChamadoService.BuscarPorId(chamado.Codigo);
            if (c.Finalizado)
            {
                throw new Exception("Chamado ja finalizado");
            }
            Finalizar(c.Eventos.OrderByDescending(x => x.Abertura).FirstOrDefault());
            evento.Abertura = DateTime.Now;
            return _iEventoRepository.Adicionar(c, evento);
        }

        public Evento AlterarDescricao(Evento evento, string descricao, Atendente atendente)
        {
            evento = _iEventoRepository.BuscarPorId(evento.Codigo);
            if (evento.Encerrado != DateTime.MinValue)
            {
                throw new Exception("Evento ja foi finalizado");
            }

            ValidarAtendenteCorrente(atendente,evento);

            evento.Descricao = descricao;
            if (evento.Descricao.Equals(""))
            {
                throw new Exception("Descricao em branco");
            }

            return _iEventoRepository.Alterar(evento);
        }

        public IEnumerable<Evento> BuscarEventosPorChamado(int codigoChamado)
        {
            return _iEventoRepository.BuscarEventosPorChamado(codigoChamado);
        }

        public Evento BuscarPorId(int codigo)
        {
            return _iEventoRepository.BuscarPorId(codigo);
        }

        public Evento Finalizar(Evento evento)
        {
            evento.Encerrado = DateTime.Now;
            return _iEventoRepository.Alterar(evento);
        }
    }
}