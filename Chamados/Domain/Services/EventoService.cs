using System;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _iEventoRepository;

        public EventoService(IEventoRepository iEventoRepository)
        {
            _iEventoRepository = iEventoRepository;
        }

        public Chamado Adicionar(Chamado chamado, Evento evento)
        {
            //validar
            var _evento = _iEventoRepository.Adicionar(chamado, evento);
            chamado.Eventos.Add(_evento);
            return chamado;

        }

        public Evento Alterar(Evento evento)
        {
            if (evento.Status.Equals("Finalizado"))
            {
                throw new Exception("Evento ja foi finalizado");
            }
            //validar
            return _iEventoRepository.Alterar(evento);
        }

        public Evento Finalizar(Evento evento)
        {
            evento.Status = "Finalizado";
            evento.Encerrado = DateTime.Now;
            return _iEventoRepository.Alterar(evento);

        }
    }
}