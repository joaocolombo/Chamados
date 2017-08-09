using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IUsuarioService
    {
        Usuario Autenticar(int id, string senha);
    }
}