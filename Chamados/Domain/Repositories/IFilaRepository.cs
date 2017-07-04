using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IFilaRepository
    {
        Fila BuscarPorId(int codigo);
        IEnumerable<Fila> BuscarFila();
    }
}