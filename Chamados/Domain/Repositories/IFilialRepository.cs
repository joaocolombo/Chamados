using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IFilialRepository
    {
        Filial BuscarPorCodigo(string codigo);
        Filial BuscarPorNome(string Nome);
        IEnumerable<Filial> BuscarListaPorNome(string Nome);
        IEnumerable<Filial> BuscarListaPorCodigo(string Codigo);
    }
}