using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IChamadoService
    {
        int Inserir(Chamado chamado);
        Chamado Alterar(Chamado chamado);
        void Finalizar(Chamado chamado);
        string Teste();
    }
}