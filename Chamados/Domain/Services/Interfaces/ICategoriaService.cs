using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface ICategoriaService

    {
        IEnumerable<Categoria> BuscarCategoria();
    }
}