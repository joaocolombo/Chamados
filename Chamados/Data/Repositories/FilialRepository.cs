using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Dapper;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class FilialRepository : IFilialRepository
    {
        public IEnumerable<Filial> BuscarListaPorCodigo(string codigo)
        {
            var sql = @"SELECT COD_FILIAL AS CODIGO, FILIAL AS NOME FROM FILIAIS WHERE COD_FILIAL LIKE @CODIGO";
            return CriativarDB.Conecection().Query<Filial>(sql, new { CODIGO = codigo });
        }

        public IEnumerable<Filial> BuscarListaPorNome(string nome)
        {
            var sql = @"SELECT COD_FILIAL AS CODIGO, FILIAL AS NOME FROM FILIAIS WHERE FILIAL LIKE @NOME";
            return CriativarDB.Conecection().Query<Filial>(sql, new { NOME = nome });
        }

        public Filial BuscarPorCodigo(string codigo)
        {
            var sql = @"SELECT COD_FILIAL AS CODIGO, FILIAL AS NOME FROM FILIAIS WHERE COD_FILIAL =@CODIGO";
            return CriativarDB.Conecection().Query<Filial>(sql, new {CODIGO = codigo}).FirstOrDefault();
        }

        public Filial BuscarPorNome(string nome)
        {
            var sql = @"SELECT COD_FILIAL AS CODIGO, FILIAL AS NOME FROM FILIAIS WHERE FILIAL =@NOME AND INATIVO=0";
            return CriativarDB.Conecection().Query<Filial>(sql, new { NOME = nome }).FirstOrDefault();
        }
    }
}