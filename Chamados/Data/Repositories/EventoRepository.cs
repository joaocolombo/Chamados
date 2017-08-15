using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;
using Dapper;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly IAtendenteRepository _iAtendenteRepository;

        public EventoRepository(IAtendenteRepository iAtendenteRepository)
        {
            _iAtendenteRepository = iAtendenteRepository;
        }

        public Evento Adicionar(Chamado chamado, Evento evento)
        {
            if (evento.Encerramento == DateTime.MinValue)
            {
                evento.Encerramento = new DateTime(1900, 01, 01);
            }

            var sql = @"INSERT INTO [CHAMADOS].[dbo].[EVENTO]
                                   ([CODIGO_STATUS]
                                   ,[ABERTURA]
                                   ,[ENCERRAMENTO]
                                   ,[ATENDENTE]
                                   ,[DESCRICAO]
                                   ,[CODIGO_CHAMADO]
                                   ,[MINUTOS_PREVISTOS]
                                   ,[MINUTOS_REALIZADOS])
                             VALUES
                                   ((SELECT CODIGO FROM EVENTO_STATUS WHERE DESCRICAO =@STATUS)
                                   , CONVERT(DATETIME,@ABERTURA,103)
                                   , CONVERT(DATETIME,@ENCERRAMENTO,103)
                                   ,@ATENDENTE
                                   ,@DESCRICAO
                                   ,@CODIGO_CHAMADO
                                   ,@MINUTOS_PREVISTOS
                                   ,@MINUTOS_REALIZADOS)
                                    SELECT SCOPE_IDENTITY ()";
            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@STATUS", evento.Status);
            comando.Parameters.AddWithValue("@ABERTURA", evento.Abertura);
            comando.Parameters.AddWithValue("@ENCERRAMENTO", evento.Encerramento);
            comando.Parameters.AddWithValue("@ATENDENTE", evento.Atendente.Nome);
            comando.Parameters.AddWithValue("@DESCRICAO", evento.Descricao);
            comando.Parameters.AddWithValue("@CODIGO_CHAMADO", chamado.Codigo);
            comando.Parameters.AddWithValue("@MINUTOS_PREVISTOS", evento.MinutosPrevistos);
            comando.Parameters.AddWithValue("@MINUTOS_REALIZADOS", evento.MinutosRealizados);
            var dr = ChamadosDb.DataReader(comando);
            dr.Read();
            evento.Codigo = Convert.ToInt32(dr[0]);
            ChamadosDb.CloseConnection();

            return evento;
        }

        public Evento Alterar(Evento evento)
        {
            if (evento.Encerramento == DateTime.MinValue)
            {
                evento.Encerramento = new DateTime(1900, 01, 01);
            }

            var sql = @"UPDATE [CHAMADOS].[dbo].[EVENTO]
                            SET [CODIGO_STATUS] = (SELECT CODIGO FROM EVENTO_STATUS WHERE DESCRICAO =@STATUS)
                                ,[ENCERRAMENTO] = @ENCERRAMENTO
                                ,[DESCRICAO] = @DESCRICAO
                                ,[MINUTOS_PREVISTOS]=@MINUTOS_PREVISTOS
                                ,[MINUTOS_REALIZADOS]=@MINUTOS_REALIZADOS
                            WHERE [CODIGO]=@CODIGO";
            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@CODIGO", evento.Codigo);
            comando.Parameters.AddWithValue("@STATUS", evento.Status);
            comando.Parameters.AddWithValue("@ENCERRAMENTO", evento.Encerramento);
            comando.Parameters.AddWithValue("@DESCRICAO", evento.Descricao);
            comando.Parameters.AddWithValue("@MINUTOS_PREVISTOS", evento.MinutosPrevistos);
            comando.Parameters.AddWithValue("@MINUTOS_REALIZADOS", evento.MinutosRealizados);
            ChamadosDb.ExecuteQueries(comando);
            ChamadosDb.CloseConnection();

            return BuscarPorId(evento.Codigo);
        }

        public List<Evento> BuscarEventosPorChamado(Evento e)
        {
            var sql = @"SELECT [CODIGO]
                      ,(SELECT DESCRICAO FROM EVENTO_STATUS WHERE CODIGO = [CODIGO_STATUS]) AS STATUS
                      ,[ABERTURA]
                      ,[ENCERRAMENTO]
                      ,[DESCRICAO]
                      ,[MINUTOS_PREVISTOS] as MINUTOSPREVISTOS
                      ,[MINUTOS_REALIZADOS] AS MINUTOSREALIZADOS
                      ,[ATENDENTE] AS NOME

                  FROM [CHAMADOS].[dbo].[EVENTO]
                WHERE [CODIGO_CHAMADO] =(SELECT CODIGO_CHAMADO FROM EVENTO WHERE CODIGO =@CODIGO)";


            var eventos = ChamadosDb.Conecection().Query<Evento, Atendente, Evento>(sql, (ev, at) =>
            {
                ev.Atendente = at;
                return ev;
            }, new { CODIGO = e.Codigo }, splitOn: "NOME").ToList();

            foreach (var evento in eventos)
                evento.Atendente = _iAtendenteRepository.BuscarAtendente(evento.Atendente.Nome);
            ChamadosDb.CloseConnection();
            return eventos;

        }
        public List<Evento> BuscarEventosPorChamado(int codigoChamado)
        {
            var sql = @"SELECT [CODIGO]
                        , (SELECT DESCRICAO FROM EVENTO_STATUS WHERE CODIGO = [CODIGO_STATUS])STATUS
                      ,[ABERTURA]
                      ,[ENCERRAMENTO]
                      ,[DESCRICAO]
                      ,[MINUTOS_PREVISTOS] as MINUTOSPREVISTOS
                      ,[MINUTOS_REALIZADOS] AS MINUTOSREALIZADOS
                      ,[ATENDENTE] AS NOME

                  FROM [CHAMADOS].[dbo].[EVENTO]
                WHERE [CODIGO_CHAMADO]  =@CODIGO";


            var eventos = ChamadosDb.Conecection().Query<Evento, Atendente, Evento>(sql, (ev, at) =>
            {
                ev.Atendente = at;
                return ev;
            }, new { CODIGO = codigoChamado }, splitOn: "NOME").ToList();

            foreach (var evento in eventos)
                evento.Atendente = _iAtendenteRepository.BuscarAtendente(evento.Atendente.Nome);
            ChamadosDb.CloseConnection();
            return eventos;

        }

        public Evento BuscarPorId(int codigo)
        {

            var sql = @"SELECT[CODIGO]
                        , (SELECT DESCRICAO FROM EVENTO_STATUS WHERE CODIGO = [CODIGO_STATUS]) AS STATUS
                        ,[ENCERRAMENTO]
                        ,[ABERTURA]
                        ,[DESCRICAO]
                      ,[MINUTOS_PREVISTOS] as MINUTOSPREVISTOS
                      ,[MINUTOS_REALIZADOS] AS MINUTOSREALIZADOS
                        ,[ATENDENTE] AS NOME

                            FROM[CHAMADOS].[dbo].[EVENTO]
                            WHERE CODIGO = @CODIGO";
            var evento = ChamadosDb.Conecection().Query<Evento, Atendente, Evento>(sql, (ev, at) =>
            {
                ev.Atendente = at;
                return ev;
            },
            new { CODIGO = codigo }, splitOn: "NOME").FirstOrDefault();

            evento.Atendente = _iAtendenteRepository.BuscarAtendente(evento.Atendente.Nome);
            ChamadosDb.CloseConnection();

            return evento;
        }

        public IEnumerable<object> BuscarStatus()
        {
            var sql = @"SELECT CODIGO
	                           ,DESCRICAO
		                        FROM EVENTO_STATUS";
            return ChamadosDb.Conecection().Query(sql).Select(x => new { id = x.DESCRICAO, text = x.DESCRICAO });
        }

        public string InserirPorChamado(IEnumerable<Evento> eventos)
        {
            var result = "";
            foreach (var evento in eventos)
            {
                if (evento.Encerramento == DateTime.MinValue)
                {
                    evento.Encerramento = new DateTime(1900, 01, 01);
                }

                result = result + (@" INSERT INTO[CHAMADOS].[dbo].[EVENTO]
                ([CODIGO_STATUS]
                ,[ABERTURA]
                ,[ENCERRAMENTO]
                ,[ATENDENTE]
                ,[DESCRICAO]
                ,[CODIGO_CHAMADO]
                ,[MINUTOS_PREVISTOS]
                ,[MINUTOS_REALIZADOS])
                VALUES
                ((SELECT CODIGO FROM EVENTO_STATUS WHERE DESCRICAO ='" + evento.Status + "')" +
                                    ", \n  CONVERT(DATETIME,'" + evento.Abertura + "',103)" +
                                    ", \n CONVERT(DATETIME,'" + evento.Encerramento + "',103)" +
                                    ", \n'" + evento.Atendente.Nome +
                                    "', \n'" + evento.Descricao +
                                            "\n ',@CODIGO_CHAMADO" +
                                            "\n ," + evento.MinutosPrevistos +
                                            "\n ," + evento.MinutosRealizados +
                                   ") ");
            }
            return result;
        }
    }
}