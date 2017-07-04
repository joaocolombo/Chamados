using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class FilaService : IFilaService
    {
        private readonly IFilaRepository _iFilaRepository;
        private readonly IChamadoService _iChamadoService;
        private readonly IEventoService _iEventoService;

        public FilaService(IFilaRepository iFilaRepository, IChamadoService iChamadoService, IEventoService iEventoService)
        {
            _iFilaRepository = iFilaRepository;
            _iChamadoService = iChamadoService;
            _iEventoService = iEventoService;
        }


        public void  AlterarFilaDoChamado(int codigo, int codigoChamado, Evento evento, Atendente atendente)
        {
            var fila = _iFilaRepository.BuscarPorId(codigo);
            var chamado = _iChamadoService.BuscarPorId(codigoChamado);
            _iEventoService.Adicionar(codigoChamado, evento, atendente);
            _iChamadoService.AlterarFila(codigoChamado, fila, atendente);

        }

        public IEnumerable<Fila> BuscarFila()
        {
            return _iFilaRepository.BuscarFila();
        }

        public Fila BuscarPorId(int codigo)
        {
            return _iFilaRepository.BuscarPorId(codigo);
        }
    }
}