using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IChamadoRepository
    {
        Chamado Alterar(Chamado chamado);
        int Inserir(Chamado chamado);
        Chamado BuscarPorId(int codigo);
        IEnumerable<Chamado> BuscarPorStatus(string status);
        IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente, bool finalizado);
        IEnumerable<Chamado> BuscarPorFilial(Filial filial);
        IEnumerable<Chamado> BuscarPorFila(Fila fila);
        Chamado BuscarPorIdEvento(int codigoEvento);
        IEnumerable<object> SelectGenerico(string tabela, string parametros, string draw, string orderby, string orderbyDirecao);
        int TotalRegistros(string tabela, string parametros);




    }
}