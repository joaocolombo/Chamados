using System.Collections;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IEventoService
    {
        Evento Adicionar(Chamado chamado, Atendente atendente, Evento evento);
        Evento Finalizar(Evento evento);
        Evento AlterarDescricao (Evento evento, string String);
        Evento BuscarPorId(int codigo);
        IEnumerable<Evento> BuscarEventosPorChamado(int codigoChamado);



    }
}