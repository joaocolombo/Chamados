using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;
using Domain.Services.Interfaces.Validates;

namespace Domain.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _iEventoRepository;
        private readonly IChamadoService _iChamadoService;
        private readonly IChamadoValidate _iChamadoValidate;
        private readonly IEventoValidate _iEventoValidate;
        private string erro;

        public EventoService(IEventoRepository iEventoRepository, IChamadoService iChamadoService, IChamadoValidate iChamadoValidate, IEventoValidate iEventoValidate )
        {
            _iChamadoValidate = iChamadoValidate;
            _iEventoRepository = iEventoRepository;
            _iChamadoService = iChamadoService;
            _iEventoValidate = iEventoValidate;
        }


        public Evento AdicionarEventoFila(int codigoChamado, Evento evento, Fila fila, Atendente atendente)
        {
            erro = _iEventoValidate.NovoEventoFila(fila);
            if (!string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }
            return Adicionar(codigoChamado, evento, atendente);
        }


        public Evento Adicionar(int codigoChamado, Evento evento, Atendente atendente)
        {
            var chamado = _iChamadoService.BuscarPorId(codigoChamado);
            erro =  _iEventoValidate.NovoEvento(evento, atendente,chamado);
            erro += _iChamadoValidate.PermiteIncluirAlterarEvento(chamado);
            _iChamadoService.RemoverFila(chamado.Codigo);
            Finalizar(chamado.Eventos.OrderByDescending(x => x.Abertura).FirstOrDefault());

            if (!string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }

            evento.Abertura = DateTime.Now;
            return _iEventoRepository.Adicionar(chamado, evento);
        }

        public Evento AlterarStatus(int codigo, string status, Atendente atendente)
        {
            var chamado = _iChamadoService.BuscarPorIdEvento(codigo);
            var evento = _iEventoRepository.BuscarPorId(codigo);
             erro = _iChamadoValidate.PermiteIncluirAlterarEvento(chamado);
            erro += _iEventoValidate.PermiteAlterarStatus(evento, atendente, status);
            if (string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }
            evento.Status = status;

            return _iEventoRepository.Alterar(evento);
        }


        public Evento AlterarDescricao(int codigo, string descricao, Atendente atendente)
        {
            var chamado = _iChamadoService.BuscarPorIdEvento(codigo);
            var evento = _iEventoRepository.BuscarPorId(codigo);
            erro = _iChamadoValidate.PermiteIncluirAlterarEvento(chamado);
            erro += _iEventoValidate.PermiteAlterarDescricao(evento, atendente, descricao);
            if (string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }
            evento.Descricao = descricao;

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

        public Evento AlterarAtendente(int codigoChamado, Atendente atendente)
        {
            var chamado = _iChamadoService.BuscarPorId(codigoChamado);
            var evento = new Evento()
            {
                Abertura = DateTime.Now,
                Atendente = atendente,
                Descricao = "Chamado Foi Assumido",
                Status = "ENCAMINHAR"
            };
            erro = _iChamadoValidate.PermiteAlterarAtendente(chamado);
            if (string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }

            _iChamadoService.RemoverFila(chamado.Codigo);
            Finalizar(chamado.Eventos.OrderByDescending(x => x.Abertura).FirstOrDefault());

            return _iEventoRepository.Adicionar(chamado, evento);
        }
    }
}