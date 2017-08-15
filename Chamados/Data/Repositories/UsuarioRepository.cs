using System;
using System.Collections.Generic;
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
            var sql = @"select  usuario Login,
                                nome NomeExibicao,
                                idUsuario Id,
	                            email Email  from usr.Usuario 
                                where idUsuario = @USUARIO and senha= @SENHA";
            var usuario =
                SgbcDB.Conecection()
                    .Query<Usuario>(sql, new {@USUARIO = id.ToString(), @SENHA = senha})
                    .FirstOrDefault();
            usuario.Grupos = ObterGrupos(id);
            return usuario;
        }

        public List<string> ObterGrupos(int id)
        {
            return SgbcDB.Conecection().Query<string>(@"SELECT C.grupo FROM USR.Usuario AS A
                                                                LEFT JOIN usr.GrupoMembro AS B ON A.idUsuario = B.idUsuario
                                                                LEFT JOIN USR.Grupo AS C ON B.idGrupo = C.idGrupo
                                                                WHERE A.idUsuario = @ID
                                                                AND C.ativo = 1", new {@ID = id}).ToList();
        }
    }
}