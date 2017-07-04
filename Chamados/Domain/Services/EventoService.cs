using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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

        private void ValidarEventoFinalizado(Evento evento)
        {
            if (evento.Encerramento != DateTime.MinValue)
            {
                throw new Exception("Evento ja foi finalizado");
            }
        }
        
        public Evento Adicionar(int codigoChamado, Evento evento, Atendente atendente)
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
            var c = _iChamadoService.BuscarPorId(codigoChamado);
            if (c.Finalizado)
            {
                throw new Exception("Chamado ja finalizado");
            }
            _iChamadoService.RemoverFila(c.Codigo);
            Finalizar(c.Eventos.OrderByDescending(x => x.Abertura).FirstOrDefault());
            evento.Abertura = DateTime.Now;
            return _iEventoRepository.Adicionar(c, evento);
        }

        public Evento AlterarStatus(int codigo, string status, Atendente atendente)
        {
            var evento = _iEventoRepository.BuscarPorId(codigo);
            ValidarEventoFinalizado(evento);
            ValidarAtendenteCorrente(atendente, evento);
            evento.Status = status;
            if (string.IsNullOrEmpty(evento.Status))
            {
                throw new Exception("Status em branco");
            }

            return _iEventoRepository.Alterar(evento);
        }


        public Evento AlterarDescricao(int codigo, string descricao, Atendente atendente)
        {
            var evento = _iEventoRepository.BuscarPorId(codigo);
            ValidarEventoFinalizado(evento);
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
            evento.Encerramento = DateTime.Now;
            return _iEventoRepository.Alterar(evento);
        }

        public IEnumerable<object> BuscarStatus()
        {
            return _iEventoRepository.BuscarStatus();
        }




    }
}