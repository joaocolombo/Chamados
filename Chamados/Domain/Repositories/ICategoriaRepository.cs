using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICategoriaRepository
    {
        string InserirSql(Chamado chamado);
        List<Categoria> BuscarCategoriasPorChamado(int codigoChamado);
        IEnumerable<Categoria> BuscarCategoria();
    }
}