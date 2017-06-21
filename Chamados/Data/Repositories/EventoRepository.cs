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
        public void Adicionar(Chamado chamado, Evento evento)
        {
            var sql = @"INSERT INTO [CHAMADOS].[dbo].[EVENTO]
                                   ([CODIGO_STATUS]
                                   ,[ABERTURA]
                                   ,[ENCERRAMENTO]
                                   ,[ATENDENTE]
                                   ,[DESCRICAO]
                                   ,[CODIGO_CHAMADO]
                                   ,[FINALIZADO])
                             VALUES
                                   (SELECT CODIGO FROM STATUS_EVENTO WHERE DESCRICAO ='@STATUS'
                                   ,@ABERTURA
                                   ,@ENCERRAMENTO
                                   ,@ATENDENTE, 
                                   ,@DESCRICAO, 
                                   ,@CODIGO_CHAMADO)";
            var comando = new SqlCommand(sql);
            comando.Parameters.AddWithValue("@STATUS", evento.Status);
            comando.Parameters.AddWithValue("@ABERTURA", evento.Abertura);
            comando.Parameters.AddWithValue("@ENCERRAMENTO", evento.Encerrado);
            comando.Parameters.AddWithValue("@ATENDENTE", evento.Atendente);
            comando.Parameters.AddWithValue("@DESCRICAO", evento.Descricao);
            comando.Parameters.AddWithValue("@CODIGO_CHAMADO", chamado.Codigo);
            var dr = ChamadosDb.DataReader(comando);
            dr.Read();
        }

        public Evento Alterar(Evento evento)
        {
            throw new NotImplementedException();
        }

        public List<Evento> BuscarEventosPorChamado(int codigoChamado)
        {
            var sql = @"SELECT [CODIGO]
                      ,[CODIGO_STATUS]
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
                    Codigo = codigoChamado,
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
                ((SELECT CODIGO FROM EVENTO_STATUS WHERE DESCRICAO ='" + evento.Status + "'), \n '" + evento.Abertura +
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