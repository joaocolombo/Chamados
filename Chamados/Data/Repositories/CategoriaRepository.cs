﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Domain.Entities;

namespace Data.Repositories
{
    public static class CategoriaRepository
    {
        public static string InserirSql(Chamado chamado)
        {
            return chamado.Categorias.Aggregate("", (current, categoria) => current + (@"
                        INSERT INTO [CHAMADOS].[dbo].[CHAMADO_CATEGORIA]
						([CODIGO_CHAMADO]
						,[CODIGO_CATEGORIA])
						VALUES
						(@CODIGO_CHAMADO
						," + categoria.Codigo + @")
                        "));
        }

        public static List<Categoria> BuscarCategoriasPorChamado(int codigoChamado)
        {
            var sql = @"SELECT B.[CODIGO],
	                           B.[DESCRICAO],
	                           C.[CODIGO],
	                           C.[DESCRICAO]	   
                          FROM [CHAMADOS].[dbo].[CHAMADO_CATEGORIA] AS A 
                          JOIN [CHAMADOS].[dbo].[CATEGORIA] AS B ON A.CODIGO_CATEGORIA =B.CODIGO
                          JOIN [CHAMADOS].[dbo].[CATEGORIA_GRUPO] AS C ON B.CODIGO_GRUPO =C.CODIGO
                          WHERE [CODIGO_CHAMADO] = @CODIGO_CHAMADO";
            

            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@CODIGO_CHAMADO", codigoChamado);
            var dr = ChamadosDb.DataReader(comando);
            List<Categoria> categorias = new List<Categoria>();
            while (dr.Read())
            {
                categorias.Add(new Categoria()
                {
                    Codigo = Convert.ToInt32(dr[0]),
                    Descricao = dr[1].ToString(),
                    Grupo = dr[3].ToString()
                });
            }

            return categorias;
        }
    }
}