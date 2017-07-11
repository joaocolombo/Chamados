
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class AtendenteRepository : IAtendenteRepository
    {
        public Atendente BuscarAtendente(string nome)
        {
            var sql = @"SELECT
                        A.usuario NOME,
                        A.idUsuario CODIGO,
                        C.grupo NIVEL,
                        D.nome +' '+D.sobrenome NOMEEXIBICAO 
                        FROM USR.Usuario AS A
                        JOIN USR.GrupoMembro AS B ON A.idUsuario=B.idUsuario
                        JOIN USR.Grupo AS C ON C.idGrupo = B.idGrupo
                        JOIN USR.UsuarioPerfil AS D ON A.idUsuario =D.idUsuario
                        WHERE B.idGrupo IN('64','65','66')
                        AND A.usuario =@NOME";
            return SgbcDB.Conecection().Query<Atendente>(sql, new { NOME = nome }).FirstOrDefault();
        }

        public Atendente BuscarAtendente(int codigo)
        {
            var sql = @"SELECT
                        A.usuario NOME,
                        A.idUsuario CODIGO,
                        C.grupo NIVEL,
                        D.nome +' '+D.sobrenome NOMEEXIBICAO 
                        FROM USR.Usuario AS A
                        JOIN USR.GrupoMembro AS B ON A.idUsuario=B.idUsuario
                        JOIN USR.Grupo AS C ON C.idGrupo = B.idGrupo
                        JOIN USR.UsuarioPerfil AS D ON A.idUsuario =D.idUsuario
                        WHERE B.idGrupo IN('64','65','66')
                        AND A.idUsuario =@CODIGO";
            return SgbcDB.Conecection().Query<Atendente>(sql, new { CODIGO = codigo }).FirstOrDefault();
        }

        public IEnumerable<Atendente> BuscarPorNivel(string nivel)
        {
            var sql = @"SELECT
                        A.usuario NOME,
                        A.idUsuario CODIGO,
                        C.grupo NIVEL,
                        D.nome +' '+D.sobrenome NOMEEXIBICAO 
                        FROM USR.Usuario AS A
                        JOIN USR.GrupoMembro AS B ON A.idUsuario=B.idUsuario
                        JOIN USR.Grupo AS C ON C.idGrupo = B.idGrupo
                        JOIN USR.UsuarioPerfil AS D ON A.idUsuario =D.idUsuario
                        WHERE B.idGrupo IN('64','65','66')
                        AND C.grupo ='NIVEL'";
            return SgbcDB.Conecection().Query<Atendente>(sql, new { NIVEL = nivel });
        }

        public IEnumerable<Atendente> BuscarTodosAtendete()
        {
            var sql = @"SELECT
                        A.usuario NOME,
                        A.idUsuario CODIGO,
                        C.grupo NIVEL,
                        D.nome +' '+D.sobrenome NOMEEXIBICAO 
                        FROM USR.Usuario AS A
                        JOIN USR.GrupoMembro AS B ON A.idUsuario=B.idUsuario
                        JOIN USR.Grupo AS C ON C.idGrupo = B.idGrupo
                        JOIN USR.UsuarioPerfil AS D ON A.idUsuario =D.idUsuario
                        WHERE B.idGrupo IN('64','65','66')";
            return SgbcDB.Conecection().Query<Atendente>(sql);
        }
    }
}