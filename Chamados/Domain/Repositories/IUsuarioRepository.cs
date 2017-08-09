using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Usuario Autenticar(int id, string senha);
    }
}