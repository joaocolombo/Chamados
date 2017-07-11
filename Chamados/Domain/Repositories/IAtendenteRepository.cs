using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IAtendenteRepository
    {
        Atendente BuscarAtendente(string nome);
        Atendente BuscarAtendente(int codigo);
        IEnumerable<Atendente> BuscarTodosAtendete();
        IEnumerable<Atendente> BuscarPorNivel(string nivel);
    }
}