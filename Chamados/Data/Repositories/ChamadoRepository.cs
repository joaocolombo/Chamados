using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Linq.Expressions;
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


        public Chamado Alterar(Chamado chamado)
        {

            var sql = @"BEGIN TRAN
                        DECLARE @CODIGO_CHAMADO INT
                        SET @CODIGO_CHAMADO =@CODIGO
                           UPDATE [CHAMADOS].[dbo].[CHAMADO]
                           SET [CODIGO_FILIAL] =@CODIGO_FILIAL
                              ,[ASSUNTO] = @ASSUNTO
                              ,[STATUS] = @STATUS
                              ,[SOLICITANTE] = @SOLICITANTE
                              ,[FINALIZADO] =@FINALIZADO
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
            ChamadosDb.ExecuteQueries(comando);
            ChamadosDb.CloseConnection();
            return BuscarPorId(chamado.Codigo);
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

            try
            {

            var chamado = ChamadosDb.Conecection().Query<Chamado, Filial, Chamado>(sql,
                (ch, fi) =>
                {
                    ch.Filial = fi;
                    return ch;
                },
             new { CODIGO = codigo }, splitOn:"-").FirstOrDefault();
            var eventos = _iEventoRepository.BuscarEventosPorChamado(codigo);
            var categorias = _iCategoriaRepository.BuscarCategoriasPorChamado(codigo);
            var filial = _iFilialRepository.BuscarPorCodigo(chamado.Filial.Codigo);
            chamado.Filial = filial;
            chamado.Eventos = eventos;
            chamado.Categorias = categorias;
            ChamadosDb.CloseConnection();

            return chamado;

            }
            catch (Exception e)
            {
               
                return null;
            }

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

            if (chamado.Imagens!=null)
            {
                foreach (var imagem in chamado.Imagens)
                {
                    sql += @"
                            INSERT INTO [Chamados].[dbo].[CHAMADO_IMAGEM]
                                                       ([CODIGO_CHAMADO]
                                                       ,[PATH_IMAGEM]
                                                       ,[NOME_IMAGEM])
                                                 VALUES
                                                       (@CODIGO_CHAMADO
                                                       ,'C:\TEMP\'
                                                       ,'"+imagem+"') ";
                }
            }




            sql += @"
                        IF @@ERROR <> 0
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

        public IEnumerable<Chamado> BuscarPorFila(Fila fila)
        {
            var sql = @"SELECT [CODIGO]
                              ,[ASSUNTO]
                              ,[STATUS]
                              ,[FINALIZADO]
                              ,[SOLICITANTE]
                              ,'-'as '-'
                              ,[CODIGO_FILIAL] AS CODIGO
                          FROM[CHAMADOS].[dbo].[CHAMADO]
                        WHERE [FILA] = @FILA";


            var chamados = ChamadosDb.Conecection().Query<Chamado, Filial, Chamado>(sql,
                (ch, fi) =>
                {
                    ch.Filial = fi;
                    return ch;
                },
             new { FILA = fila.Codigo }, splitOn: "-");

            foreach (var chamado in chamados)
            {
                var eventos = _iEventoRepository.BuscarEventosPorChamado(chamado.Codigo);
                var categorias = _iCategoriaRepository.BuscarCategoriasPorChamado(chamado.Codigo);
                var filial = _iFilialRepository.BuscarPorCodigo(chamado.Filial.Codigo);
                chamado.Filial = filial;
                chamado.Eventos = eventos;
                chamado.Categorias = categorias;
            }

            ChamadosDb.CloseConnection();

            return chamados;
        }

        public Chamado BuscarPorIdEvento(int codigoEvento)
        {
            var sql = @"SELECT A.[CODIGO]
                                  ,[CODIGO_FILIAL]
                                  ,[ASSUNTO]
                                  ,[STATUS]
                                  ,[FINALIZADO]
                                  ,[SOLICITANTE]
                              FROM [Chamados].[dbo].[CHAMADO] AS A
                              JOIN EVENTO AS B ON A.CODIGO=B.CODIGO_CHAMADO 
                              WHERE B.CODIGO=@CODIGOEVENTO";
            return ChamadosDb.Conecection().Query<Chamado>(sql, new {CODIGOEVENTO = codigoEvento}).FirstOrDefault();
        }

        //------

        public IEnumerable<object> SelectGenerico(string tabela, string parametros, string draw, string orderby, string orderbyDirecao )
        {
            var sql =@"select COLUNA.name from SYS.columns AS COLUNA
                        JOIN SYS.tables AS TABELA ON COLUNA.object_id = TABELA.object_id WHERE TABELA.name ='"+ tabela+"'";

            var colunas = ChamadosDb.Conecection().Query<string>(sql);
            
            sql = "SELECT top("+draw+") * FROM " + tabela;

            if (!string.IsNullOrEmpty(parametros))
            {
                sql += " where ";
                foreach (var coluna in colunas)
                {
                    sql += coluna + " like '%" + parametros + "%' or ";
                }
                sql += " 0=1";
            }

            return ChamadosDb.Conecection().Query(sql);
        }

        public int TotalRegistros(string tabela, string parametros)
        {
            throw new NotImplementedException();
        }

        public Chamado AdicionarNaFila(int codigo, Fila fila)
        {
            var sql = @"UPDATE CHAMADO SET FILA = @FILA WHERE CODIGO =@CODIGO";
            var retornoUpdate =ChamadosDb.Conecection().Execute(sql, new {FILA = fila.Codigo, CODIGO = codigo});
            if (retornoUpdate!=1)
            {
                throw new Exception("Ocorreu um erro na alteração");
            }
            return BuscarPorId(codigo);
        }

        public bool ChamadoEmFila(int codigoChamado)
        {
            var sql = @"SELECT ISNULL(FILA,0) FILA FROM CHAMADO WHERE CODIGO=@CODIGO";
            var retorno = ChamadosDb.Conecection().Query<int>(sql, new { CODIGO = codigoChamado }).FirstOrDefault();
            return retorno !=0;
        }

        public Chamado RemoveDaFila(int codigo)
        {
            var sql = @"UPDATE CHAMADO SET FILA = null WHERE CODIGO =@CODIGO";
            var retornoUpdate = ChamadosDb.Conecection().Execute(sql, new { CODIGO = codigo });
            if (retornoUpdate != 1)
            {
                throw new Exception("Ocorreu um erro na alteração");
            }
            return BuscarPorId(codigo);
        }

        public void AdicionarImagem(int codigo, string nomeArquivo)
        {
            try
            {
                var sql = @"INSERT INTO [Chamados].[dbo].[CHAMADO_IMAGEM]
                                                       ([CODIGO_CHAMADO]
                                                       ,[PATH_IMAGEM]
                                                       ,[NOME_IMAGEM])
                                                 VALUES
                                                       (@CODIGO
                                                       ,'C:\TEMP\'
                                                       ,@NOMEARQUIVO)";

                ChamadosDb.Conecection().Execute(sql, new{ CODIGO =codigo, NOMEARQUIVO=nomeArquivo});
;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<string> BuscarImagensPorChamado(int codigo)
        {
            var sql = @"select nome_imagem from chamado_imagem where codigo_chamado =@CHAMADO";
            return ChamadosDb.Conecection().Query<string>(sql, new {@CHAMADO =codigo}).ToList();
        }
    }
}