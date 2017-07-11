using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Interfaces.Validates

{
    public interface IEventoValidate
    {
        string NovoEvento(Evento evento, Atendente atendente, Chamado chamado);
        string PermiteAlterarStatus(Evento evento, Atendente atendente, string status);
        string PermiteAlterarDescricao(Evento evento, Atendente atendente, string descricao);
        string NovoEventoFila(Fila fila);
        string PermiteAlterarAtendente(Evento evento);
        //        string PermiteAlterarCategoria(Chamado chamado, Atendente atendente, List<Categoria> categorias);
        //        string PermiteAlterarFilial(Chamado chamado, Atendente atendente, Filial filial);
        //        string PermiteAlterarFila(Chamado chamado, Atendente atendente, Fila fila);
        //        string PermiteFinalizar(Chamado chamado, Atendente atendente);
    }
}