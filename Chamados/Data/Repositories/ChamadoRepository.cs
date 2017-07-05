using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Dapper;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class ChamadoRepository : IChamadoRepository
    {
        private readonly IEventoRepository _iEventoRepository;
        private readonly IFilialRepository _iFilialRepository;
        private readonly ICategoriaRepository _iCategoriaRepository;

        public ChamadoRepository(IEventoRepository iEventoRepository, IFilialRepository iFilialRepository, ICategoriaRepository iCategoriaRepository)
        {
            _iEventoRepository = iEventoRepository;
            _iFilialRepository = iFilialRepository;
            _iCategoriaRepository = iCategoriaRepository;
        }


        private Chamado VerificaFila(Chamado chamado)
        {
            if (chamado.Fila == null)
                chamado.Fila = new Fila() { Codigo = 0 };
            return chamado;
        }

        public Chamado Alterar(Chamado chamado)
        {
            chamado = VerificaFila(chamado);

            var sql = @"BEGIN TRAN
                        DECLARE @CODIGO_CHAMADO INT
                        SET @CODIGO_CHAMADO =@CODIGO
                           UPDATE [CHAMADOS].[dbo].[CHAMADO]
                           SET [CODIGO_FILIAL] =@CODIGO_FILIAL
                              ,[ASSUNTO] = @ASSUNTO
                              ,[STATUS] = @STATUS
                              ,[SOLICITANTE] = @SOLICITANTE
                              ,[FINALIZADO] =@FINALIZADO
                              ,[FILA] =@FILA
                         WHERE [CODIGO]=@CODIGO
                        
                        DELETE FROM [CHAMADOS].[dbo].[CHAMADO_CATEGORIA]
                      WHERE [CODIGO_CHAMADO]=@CODIGO
                        ";
            sql += _iCategoriaRepository.InserirSql(chamado);
            sql += @" IF @@ERROR <> 0
                        ROLLBACK
                        ELSE
                         COMMIT";

            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@CODIGO_FILIAL", chamado.Filial.Codigo);
            comando.Parameters.AddWithValue("@ASSUNTO", chamado.Assunto);
            comando.Parameters.AddWithValue("@STATUS", chamado.Status);
            comando.Parameters.AddWithValue("@CODIGO", chamado.Codigo);
            comando.Parameters.AddWithValue("@SOLICITANTE", chamado.Solicitante);
            comando.Parameters.AddWithValue("@FINALIZADO", chamado.Finalizado);
            comando.Parameters.AddWithValue("@FILA", chamado.Fila.Codigo);
            ChamadosDb.ExecuteQueries(comando);
            ChamadosDb.CloseConnection();
            return chamado;
        }

        public IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente, bool finalizado)
        {
            var sql = @"SELECT
                        A.CODIGO
                        ,A.ASSUNTO
                        ,A.FINALIZADO
                        ,A.STATUS
                        ,A.SOLICITANTE
                        ,'FILIAL' AS FILIAL
                        , A.CODIGO_FILIAL AS CODIGO
                        FROM W_ULTIMO_EVENTO_CHAMADO AS A
                        WHERE ATENDENTE =@ATENDENTE
                        AND FINALIZADO =@FINALIZADO";
            var chamados = ChamadosDb.Conecection().Query<Chamado, Filial, Chamado>(sql,
                (ch, fi) =>
                {
                    ch.Filial = fi;
                    return ch;
                }, new { ATENDENTE = atendente.Nome, FINALIZADO = finalizado }, splitOn: "FILIAL").ToList();

            foreach (var chamado in chamados)
            {
                chamado.Filial = _iFilialRepository.BuscarPorCodigo(chamado.Filial.Codigo);
                chamado.Eventos = _iEventoRepository.BuscarEventosPorChamado(chamado.Codigo);
                chamado.Categorias = _iCategoriaRepository.BuscarCategoriasPorChamado(chamado.Codigo);

            }
            ChamadosDb.CloseConnection();

            return chamados;
        }

        public IEnumerable<Chamado> BuscarPorFilial(Filial filial)
        {
            throw new NotImplementedException();
        }

        public Chamado BuscarPorId(int codigo)
        {
            var sql = @"SELECT [CODIGO]
                              ,[ASSUNTO]
                              ,[STATUS]
                              ,[FINALIZADO]
                              ,[SOLICITANTE]
                              ,'-'as '-'
                              ,[CODIGO_FILIAL] AS CODIGO
                          FROM[CHAMADOS].[dbo].[CHAMADO]
                        WHERE [CODIGO] = @CODIGO";


            var chamado = ChamadosDb.Conecection().Query<Chamado, Filial, Chamado>(sql,
                (ch, fi) =>
                {
                    ch.Filial = fi;
                    return ch;
                },
             new { CODIGO = codigo }, splitOn: "-").FirstOrDefault();
            var eventos = _iEventoRepository.BuscarEventosPorChamado(codigo);
            var categorias = _iCategoriaRepository.BuscarCategoriasPorChamado(codigo);
            var filial = _iFilialRepository.BuscarPorCodigo(chamado.Filial.Codigo);
            chamado.Filial = filial;
            chamado.Eventos = eventos;
            chamado.Categorias = categorias;
            ChamadosDb.CloseConnection();

            return chamado;


        }

        public IEnumerable<Chamado> BuscarPorStatus(string status)
        {
            throw new NotImplementedException();
        }

        public int Inserir(Chamado chamado)
        {
            chamado = VerificaFila(chamado);
            var sql = @"BEGIN TRAN
                        DECLARE @CODIGO_CHAMADO INT
                        INSERT INTO [CHAMADOS].[dbo].[CHAMADO]
                       ([CODIGO_FILIAL]
                       ,[ASSUNTO]
                       ,[STATUS]
                       ,[SOLICITANTE]
                       ,[FINALIZADO])
                        VALUES
                       (@CODIGO_FILIAL
                       ,@ASSUNTO
                       ,@STATUS
                       ,@SOLICITANTE               
                       ,@FINALIZADO)
                     SET @CODIGO_CHAMADO =(SELECT SCOPE_IDENTITY ())
                       ";
            sql += _iCategoriaRepository.InserirSql(chamado);
            sql += _iEventoRepository.InserirPorChamado(chamado.Eventos);

            sql += @"IF @@ERROR <> 0
                        ROLLBACK
                        ELSE
                        BEGIN
                            COMMIT
                            SELECT @CODIGO_CHAMADO
                        END";
            try
            {
                var comando = new SqlCommand(sql);
                comando.Parameters.AddWithValue("@CODIGO_FILIAL", chamado.Filial.Codigo);
                comando.Parameters.AddWithValue("@ASSUNTO", chamado.Assunto);
                comando.Parameters.AddWithValue("@STATUS", chamado.Status);
                comando.Parameters.AddWithValue("@SOLICITANTE", chamado.Solicitante);
                comando.Parameters.AddWithValue("@FINALIZADO", chamado.Finalizado);
                var dr = ChamadosDb.DataReader(comando);
                dr.Read();
                var retorno = Convert.ToInt32(dr[0]);
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