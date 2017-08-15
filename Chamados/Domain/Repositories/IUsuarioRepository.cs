using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Usuario Autenticar(int id, string senha);
        List<string> ObterGrupos(int id);
    }
}