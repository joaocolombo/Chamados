using System;
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

        public Chamado Adicionar(Chamado chamado, Evento evento)
        {
            //validar
            var c = _iChamadoService.BuscarPorId(chamado.Codigo);
            if (!c.Eventos.OrderByDescending(x => x.Abertura).FirstOrDefault().Status.Equals("ENCERRADO"))
            {
                throw new Exception("Ultimo evento não foi encerrao");
            }
            var _evento = _iEventoRepository.Adicionar(chamado, evento);
            chamado.Eventos.Add(_evento);
            return chamado;
        }

        public Evento AlterarDescricao(Evento evento, string descricao)
        {
            evento = _iEventoRepository.BuscarPorId(evento.Codigo);
            if (evento.Status.Equals("Finalizado"))
            {
                throw new Exception("Evento ja foi finalizado");
            }
            evento.Descricao = descricao;
            if (evento.Descricao.Equals(""))
            {
                throw new Exception("Descricao em branco");
            }

            return _iEventoRepository.Alterar(evento);
        }

        public Evento BuscarPorId(int codigo)
        {
            throw new NotImplementedException();
        }

        public Evento Finalizar(Evento evento)
        {
            evento.Status = "Finalizado";
            evento.Encerrado = DateTime.Now;
            return _iEventoRepository.Alterar(evento);
        }
    }
}