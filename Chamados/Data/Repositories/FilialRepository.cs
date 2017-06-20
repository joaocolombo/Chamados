using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class FilialRepository : IFilialRepository
    {
        public IEnumerable<Filial> BuscarListaPorCodigo(string Codigo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Filial> BuscarListaPorNome(string Nome)
        {
            throw new NotImplementedException();
        }

        public Filial BuscarPorCodigo(string codigo)
        {
            return new Filial() {Codigo = "90030", Empresa = 30, Nome = "Maringa"};
        }

        public Filial BuscarPorNome(string Nome)
        {
            throw new NotImplementedException();
        }
    }
}