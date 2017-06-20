using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        public Evento Adicionar(Chamado chamado, Evento evento)
        {
            throw new NotImplementedException();
        }

        public Evento Alterar(Evento evento)
        {
            throw new NotImplementedException();
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
                ([STATUS]
                ,[ABERTURA]
                ,[ENCERRAMENTO]
                ,[ATENDENTE]
                ,[DESCRICAO]
                ,[CODIGO_CHAMADO])
                VALUES
                ('" + evento.Status + "', \n '" + evento.Abertura +
                                   "', \n '" + evento.Encerrado +
                                   "', \n'" + evento.Atendente.Nome + "', \n'" + evento.Descricao + @"', 
                    @CODIGO_CHAMADO)
                ");
            }
            return result;
        }
    }
}