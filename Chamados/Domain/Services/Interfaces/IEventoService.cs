using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IEventoService
    {
        Chamado Adicionar(Chamado chamado, Evento evento);
        Evento Finalizar(Evento evento);
        Evento Alterar(Evento evento);


    }
}