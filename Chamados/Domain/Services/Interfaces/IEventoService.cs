using System.Collections;
using System.Collections.Generic;
using System.IO;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IEventoService
    {
        Evento Adicionar(int codigoChamado, Evento evento, Atendente atendente);
        Evento Finalizar(Evento evento);
        Evento AlterarDescricao (int codigo, string descricao, Atendente antendente);
        Evento AlterarStatus (int codigo, string status, Atendente antendente);
        Evento AlterarAtendente (int codigoChamado ,Atendente antendente);
        Evento BuscarPorId(int codigo);
        IEnumerable<Evento> BuscarEventosPorChamado(int codigoChamado);
        IEnumerable<object> BuscarStatus();
        Evento AdicionarEventoFila(int codigoChamado, Evento evento, Fila fila, Atendente atendente);


    }
}