using Domain.Entities;
using MVC.ViewModel.Evento;

namespace MVC.Mapper
{
    public static class EventoTo
    {
        public static AdicionarEventoViewModel AdicionarEventoViewModel(Evento evento)
        {
            return new AdicionarEventoViewModel()
            {
                Descricao = evento.Descricao,
                Status = evento.Status,
                Abertura = evento.Abertura,
                Atendente = evento.Atendente,
                Codigo = evento.Codigo,
                FilaId = evento.Codigo,
                MinutosPrevistos = evento.MinutosPrevistos,
                MinutosRealizados = evento.MinutosRealizados

            };
        }
    }
}