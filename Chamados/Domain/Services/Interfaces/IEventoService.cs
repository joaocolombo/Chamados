using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IEventoService
    {
        Chamado Adicionar(Chamado chamado, Evento evento);
        Evento Finalizar(Evento evento);
        Evento Alterar(Evento evento);
        void EncaminharN2(Evento evento, Chamado chamado);
        void Encaminhar(Evento evento, Atendente atendente, Chamado chamado);


    }
}