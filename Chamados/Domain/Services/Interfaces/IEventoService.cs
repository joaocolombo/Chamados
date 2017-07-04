using System.Collections;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IEventoService
    {
        Evento Adicionar(int codigo, Evento evento, Atendente atendente);
        Evento Finalizar(Evento evento);
        Evento AlterarDescricao (int codigo, string descricao, Atendente antendente);
        Evento AlterarStatus (int codigo, string status, Atendente antendente);
        Evento BuscarPorId(int codigo);
        IEnumerable<Evento> BuscarEventosPorChamado(int codigoChamado);
        IEnumerable<object> BuscarStatus();



    }
}