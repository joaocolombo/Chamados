﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class ChamadoRepository : IChamadoRepository
    {
        private readonly IEventoRepository _iEventoRepository;
        private readonly IFilialRepository _iFilialRepository;

        public ChamadoRepository(IEventoRepository iEventoRepository, IFilialRepository iFilialRepository)
        {
            _iEventoRepository = iEventoRepository;
            _iFilialRepository = iFilialRepository;
        }


        public Chamado Alterar(Chamado chamado)
        {
            var sql = @"BEGIN TRAN
                           UPDATE [CHAMADOS].[dbo].[CHAMADO]
                           SET [CODIGO_FILIAL] =@CODIGO_FILIAL
                              ,[ASSUNTO] = @ASSUNTO
                              ,[STATUS] = @STATUS
                         WHERE [CODIGO]=@CODIGO
                        
                        DELETE FROM [CHAMADOS].[dbo].[CHAMADO_CATEGORIA]
                      WHERE @CODIGO
                        ";
            CategoriaRepository.InserirSql(chamado);
            sql += " COMMIT";

            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@CODIGO_FILIAL", chamado.Filial.Codigo);
            comando.Parameters.AddWithValue("@ASSUNTO", chamado.Assunto);
            comando.Parameters.AddWithValue("@STATUS", chamado.Status);
            comando.Parameters.AddWithValue("@CODIGO", chamado.Codigo);
            ChamadosDb.ExecuteQueries(comando);

            return chamado;
        }

        public IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente, string status)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Chamado> BuscarPorFilial(Filial filial)
        {
            throw new NotImplementedException();
        }

        public Chamado BuscarPorId(int codigo)
        {
            var sql = @"SELECT [CODIGO]
                              ,[CODIGO_FILIAL]
                              ,[ASSUNTO]
                              ,[STATUS]
                          FROM[CHAMADOS].[dbo].[CHAMADO]
                        WHERE [CODIGO] = @CODIGO";

            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@CODIGO",codigo);
            var dr = ChamadosDb.DataReader(comando);
            dr.Read();
            var chamado = new Chamado
            {
                Codigo = Convert.ToInt32(dr[0]),
                Assunto = dr[2].ToString(),
                Status = dr[3].ToString(),
                Filial = _iFilialRepository.BuscarPorCodigo(dr[1].ToString()),
                Eventos = _iEventoRepository.BuscarEventosPorChamado(codigo),
                Categorias = CategoriaRepository.

            };
            chamado.Codigo = codigo;

            ChamadosDb.CloseConnection();

            return chamado;

        }

        public IEnumerable<Chamado> BuscarPorStatus(string status)
        {
            throw new NotImplementedException();
        }

        public int Inserir(Chamado chamado)
        {
            var sql = @"BEGIN TRAN
                        DECLARE @CODIGO_CHAMADO INT
                        INSERT INTO [CHAMADOS].[dbo].[CHAMADO]
                       ([CODIGO_FILIAL]
                       ,[ASSUNTO]
                       ,[STATUS])
                        VALUES
                       (@CODIGO_FILIAL
                       ,@ASSUNTO
                       ,@STATUS)
                     SET @CODIGO_CHAMADO =(SELECT SCOPE_IDENTITY ())
                       ";
            sql += CategoriaRepository.InserirSql(chamado);
            sql += _iEventoRepository.InserirPorChamado(chamado.Eventos);
    
            sql += "COMMIT SELECT @CODIGO_CHAMADO";
            try
            {
            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@CODIGO_FILIAL", chamado.Filial.Codigo);
            comando.Parameters.AddWithValue("@ASSUNTO", chamado.Assunto);
            comando.Parameters.AddWithValue("@STATUS", chamado.Status);
            var dr =ChamadosDb.DataReader(comando);
            dr.Read();
            var retorno =Convert.ToInt32(dr[0]); 
            ChamadosDb.CloseConnection();
            return retorno;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

    }
}