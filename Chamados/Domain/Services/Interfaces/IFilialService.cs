using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IFilialService
    {
        Filial BuscarPorCodigo(string codigo);
        Filial BuscarPorNome(string Nome);
        IEnumerable<Filial> BuscarListaPorNome(string Nome);
        IEnumerable<Filial> BuscarListaPorCodigo(string Codigo);
    }
}