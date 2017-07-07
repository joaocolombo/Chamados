using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Interfaces.Validates
{
    public interface IChamadoValidate
    {
        string NovoChamado(Chamado chamado);
        string Finalizado(Chamado chamado);
        string AtendenteCorrente(Chamado chamado, Atendente atendente);
        string PermiteAlterarAssunto(Chamado chamado, Atendente atendente, string assunto);
        string PermiteAlterarSolicitante(Chamado chamado, Atendente atendente, string solicitante);
        string PermiteAlterarCategoria(Chamado chamado, Atendente atendente, List<Categoria> categorias );
        string PermiteAlterarFilial(Chamado chamado, Atendente atendente, Filial filial);
        string PermiteAlterarFila(Chamado chamado, Atendente atendente, Fila fila);
        string PermiteFinalizar(Chamado chamado, Atendente atendente);
        string PermiteIncluirAlterarEvento(Chamado chamado);
        string PermiteAlterarAtendente(Chamado chamado);
    }
}