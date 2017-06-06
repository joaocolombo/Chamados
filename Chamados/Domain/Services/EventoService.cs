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

        public void Encaminhar(Evento evento, Atendente atendente, Chamado chamado)
        {
            //finalizar evento anterior
            evento.Descricao +=" Encaminhado para o Atendente " + atendente.Nome;
            Adicionar(chamado, evento);
        }

        public void EncaminharN2(Evento evento, Chamado chamado)
        {
            //finalizar evento anterior
            evento.Descricao += " Encaminhado para a Fila do N2";
            Adicionar(chamado, evento);
        }

        public Evento Finalizar(Evento evento)
        {
            evento.Status = "Finalizado";
            evento.Encerrado = DateTime.Now;
            return _iEventoRepository.Alterar(evento);
        }
    }
}