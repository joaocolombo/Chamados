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
        IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente, string status);
        IEnumerable<Chamado> BuscarPorFilial(Filial filial);
    }
}