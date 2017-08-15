
using System.Collections.Generic;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class FilialService: IFilialService
    {
        private readonly IFilialRepository _iFilialRepository;

        public FilialService(IFilialRepository iFilialRepository)
        {
            _iFilialRepository = iFilialRepository;
        }

        public IEnumerable<Filial> BuscarListaPorCodigo(string codigo)
        {
            return _iFilialRepository.BuscarListaPorCodigo(codigo);
        }

        public IEnumerable<Filial> BuscarListaPorNome(string nome)
        {
            return _iFilialRepository.BuscarListaPorNome(nome);
        }

        public Filial BuscarPorCodigo(string codigo)
        {
            return _iFilialRepository.BuscarPorCodigo(codigo);
        }

        public Filial BuscarPorNome(string nome)
        {
            return _iFilialRepository.BuscarPorNome(nome);
        }
    }
}