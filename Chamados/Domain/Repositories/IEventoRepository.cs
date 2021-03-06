﻿using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEventoRepository
    {
        Evento Alterar(Evento evento);
        Evento Adicionar(Chamado chamado, Evento evento);
        Evento BuscarPorId(int codigo);
        string InserirPorChamado(IEnumerable<Evento> eventos);
        List<Evento> BuscarEventosPorChamado(Evento evento);
        List<Evento> BuscarEventosPorChamado(int codigoChamado);
        IEnumerable<object> BuscarStatus();


    }
}