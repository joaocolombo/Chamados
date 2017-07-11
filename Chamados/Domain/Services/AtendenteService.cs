using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class AtendenteService : IAtendenteService
    {
        private readonly IAtendenteRepository _iAtendenteRepository;

        public AtendenteService(IAtendenteRepository iAtendenteRepository)
        {
            _iAtendenteRepository = iAtendenteRepository;
        }

        public Atendente BuscarAtendente(string nome)
        {
            return _iAtendenteRepository.BuscarAtendente(nome);
        }

        public Atendente BuscarAtendente(int codigo)
        {
            return _iAtendenteRepository.BuscarAtendente(codigo);
        }

        public IEnumerable<Atendente> BuscarPorNivel(string nivel)
        {
            return _iAtendenteRepository.BuscarPorNivel(nivel);
        }

        public IEnumerable<Atendente> BuscarTodosAtendete()
        {
            return _iAtendenteRepository.BuscarTodosAtendete();
        }
    }
}