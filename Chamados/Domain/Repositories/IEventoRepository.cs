using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEventoRepository
    {
        Evento Alterar(Evento evento);
        Evento Adicionar(Chamado chamado, Evento evento);
    }
}