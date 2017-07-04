using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IFilaService
    {
        Fila BuscarPorId(int codigo);
        IEnumerable<Fila> BuscarFila();
        void AlterarFilaDoChamado(int codigo, int codigoChamado, Evento evento, Atendente atendente);
    }
}