using System.Collections;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IChamadoService
    {
        int Inserir(Chamado chamado);
        Chamado Alterar(Chamado chamado);
        void Finalizar(Chamado chamado);
        Chamado BuscarPorId(int codigo);
        string Teste();
        IEnumerable<Chamado> BuscarPorStatus(string status);
        IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente, string status);
        IEnumerable<Chamado> BuscarPorFilial(Filial filial);
    }
}