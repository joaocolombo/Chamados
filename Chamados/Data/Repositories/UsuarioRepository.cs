using System.Linq;
using Dapper;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class UsuarioRepository:IUsuarioRepository
    {
        public Usuario Autenticar(int id, string senha)
        {
            var sql = "select*from usr.Usuario where idUsuario = @USUARIO and senha= @SENHA";
            return  SgbcDB.Conecection().Query<Usuario>(sql, new { @USUARIO = id.ToString(), @SENHA = senha }).FirstOrDefault();
            
        }
    }
}