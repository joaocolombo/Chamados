using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        public Evento Adicionar(Chamado chamado, Evento evento)
        {
            if (evento.Encerrado == DateTime.MinValue)
            {
                evento.Encerrado = new DateTime(1990, 01, 01);
            }

            var sql = @"INSERT INTO [CHAMADOS].[dbo].[EVENTO]
                                   ([CODIGO_STATUS]
                                   ,[ABERTURA]
                                   ,[ENCERRAMENTO]
                                   ,[ATENDENTE]
                                   ,[DESCRICAO]
                                   ,[CODIGO_CHAMADO])
                             VALUES
                                   ((SELECT CODIGO FROM EVENTO_STATUS WHERE DESCRICAO =@STATUS)
                                   ,@ABERTURA
                                   ,@ENCERRAMENTO
                                   ,@ATENDENTE
                                   ,@DESCRICAO
                                   ,@CODIGO_CHAMADO)
                                    SELECT SCOPE_IDENTITY ()";
            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@STATUS", evento.Status);
            comando.Parameters.AddWithValue("@ABERTURA", evento.Abertura);
            comando.Parameters.AddWithValue("@ENCERRAMENTO", evento.Encerrado);
            comando.Parameters.AddWithValue("@ATENDENTE", evento.Atendente.Nome);
            comando.Parameters.AddWithValue("@DESCRICAO", evento.Descricao);
            comando.Parameters.AddWithValue("@CODIGO_CHAMADO", chamado.Codigo);
            var dr = ChamadosDb.DataReader(comando);
            dr.Read();
            evento.Codigo = Convert.ToInt32(dr[0]);

            return evento;
        }

        public Evento Alterar(Evento evento)
        {
            if (evento.Encerrado == DateTime.MinValue)
            {
                evento.Encerrado = new DateTime(1990, 01, 01);
            }

            var sql = @"UPDATE [CHAMADOS].[dbo].[EVENTO]
                            SET [CODIGO_STATUS] = (SELECT CODIGO FROM EVENTO_STATUS WHERE DESCRICAO =@STATUS)
                                ,[ENCERRAMENTO] = @ENCERRAMENTO
                                ,[DESCRICAO] = @DESCRICAO
                            WHERE [CODIGO]=@CODIGO";
            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@CODIGO", evento.Codigo);
            comando.Parameters.AddWithValue("@STATUS", evento.Status);
            comando.Parameters.AddWithValue("@ENCERRAMENTO", evento.Encerrado);
            comando.Parameters.AddWithValue("@DESCRICAO", evento.Descricao);
            ChamadosDb.ExecuteQueries(comando);
            return evento;
        }

        public List<Evento> BuscarEventosPorChamado(int codigoChamado)
        {
            var sql = @"SELECT [CODIGO]
                      ,SELECT DESCRICAO FROM Z[CODIGO_STATUS]
                      ,[ABERTURA]
                      ,[ENCERRAMENTO]
                      ,[ATENDENTE]
                      ,[DESCRICAO]
                      ,[CODIGO_CHAMADO]
                  FROM [CHAMADOS].[dbo].[EVENTO]
                WHERE [CODIGO_CHAMADO] =@CODIGO_CHAMADO";
            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@CODIGO_CHAMADO", codigoChamado);
            var dr = ChamadosDb.DataReader(comando);
            List<Evento> eventos = new List<Evento>();
            while (dr.Read())
            {
                eventos.Add(new Evento()
                {
                    Codigo = Convert.ToInt32(dr[0]),
                    Status = dr[1].ToString(),
                    Abertura = Convert.ToDateTime(dr[2]),
                    Encerrado = Convert.ToDateTime(dr[3]),
                    Atendente = AtendenteRepository.BuscarAtendente(dr[4].ToString()),
                    Descricao = dr[5].ToString()
                });
            }
            ChamadosDb.CloseConnection();

            return eventos;
        }

        public Evento BuscarPorId(int codigo)
        {
            throw new NotImplementedException();
        }

        public string InserirPorChamado(IEnumerable<Evento> eventos)
        {
            var result = "";
            foreach (var evento in eventos)
            {
                if (evento.Encerrado == DateTime.MinValue)
                {
                    evento.Encerrado = new DateTime(1990, 01, 01);
                }

                result = result + (@" INSERT INTO[CHAMADOS].[dbo].[EVENTO]
                ([CODIGO_STATUS]
                ,[ABERTURA]
                ,[ENCERRAMENTO]
                ,[ATENDENTE]
                ,[DESCRICAO]
                ,[CODIGO_CHAMADO])
                VALUES
                ((SELECT CODIGO FROM EVENTO_STATUS WHERE DESCRICAO ='" + evento.Status + "')" +
                                    ", \n '" + evento.Abertura +
                                    "', \n '" + evento.Encerrado +
                                    "', \n'" + evento.Atendente.Nome +
                                    "', \n'" + evento.Descricao +
                    "',@CODIGO_CHAMADO) \n" +
                                   " ");
            }
            return result;
        }
    }
}