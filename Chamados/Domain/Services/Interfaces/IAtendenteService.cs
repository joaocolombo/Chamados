using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IAtendenteService
    {
        Atendente BuscarAtendente(string nome);
        Atendente BuscarAtendente(int codigo);
        IEnumerable<Atendente> BuscarTodosAtendete();
        IEnumerable<Atendente> BuscarPorNivel(string nivel);

    }
}