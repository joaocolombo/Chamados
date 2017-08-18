using System.Collections.Generic;
using Domain.Entities;
using MVC.ViewModel.Evento;
using MVC.ViewModel.Home;

namespace MVC.Mapper
{
    public static class ChamadoTo
    {
        public static ChamadoViewModel ChamadoViewModel(Chamado chamado)
        {
            List<AdicionarEventoViewModel> eventos = new List<AdicionarEventoViewModel>();
            foreach (var evento in chamado.Eventos)
            {
                eventos.Add(new AdicionarEventoViewModel()
                {
                    Descricao = evento.Descricao,
                    Status = evento.Status,
                    Abertura = evento.Abertura,
                    Atendente = evento.Atendente,
                    Codigo = evento.Codigo,
                    ChamadoId = chamado.Codigo,
                    FilaId = evento.Codigo,
                    MinutosPrevistos = evento.MinutosPrevistos,
                    MinutosRealizados = evento.MinutosRealizados
                });
            }
            return new ChamadoViewModel()
            {
                Assunto = chamado.Assunto,
                Codigo = chamado.Codigo,
                Categorias = chamado.Categorias,
                Filial = chamado.Filial,
                Finalizado = chamado.Finalizado,
                Solicitante = chamado.Solicitante,
                Status = chamado.Status,
                Eventos = eventos
            };
        }
    }
}