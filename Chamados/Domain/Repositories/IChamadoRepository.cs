using Domain.Entities;

namespace Domain.Repositories
{
    public interface IChamadoRepository
    {
        Chamado Alterar(Chamado chamado);
        int Inserir(Chamado chamado);
        Chamado BuscarPorId(int codigo);
    }
}