using System;
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

        public ChamadoRepository(IEventoRepository iEventoRepository)
        {
            _iEventoRepository = iEventoRepository;
        }

        private Chamado CriarChamado()
        {
            var codigo = 6;
            var a1 = new Atendente() { Nome = "Joao", Nivel = "N2" };
            var a2 = new Atendente() { Nome = "Douglas", Nivel = "N1" };
            var e1 = new Evento()
            {
                Abertura = DateTime.Now.AddDays(-2),
                Atendente = a2,
                Descricao = "evento1",
                Encerrado = DateTime.Now.AddDays(-1),
                Status = "ENCERRADO"
            };
            var e2 = new Evento()
            {
                Abertura = DateTime.Now.AddDays(-1),
                Atendente = a2,
                Descricao = "Encaminhado N2",
                Encerrado = DateTime.Now,
                Status = "ENCERRADO"
            };
            var e3 = new Evento()
            {
                Abertura = DateTime.Now,
                Atendente = a1,
                Descricao = "evento 3",
                Status = "ABERTO"
            };
            List<Evento> lista = new List<Evento>();
            lista.Add(e1);
            lista.Add(e2);
            lista.Add(e3);

            return new Chamado() { Codigo = codigo, Eventos = lista, Status = "ABERTO"};

        }

        public Chamado Alterar(Chamado chamado)
        {
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
            return CriarChamado();
            
        }

        public IEnumerable<Chamado> BuscarPorStatus(string status)
        {
            List<Chamado> listaChamados = new List<Chamado>
            {
                CriarChamado(),
                CriarChamado(),
                CriarChamado(),
                CriarChamado()
            };

            listaChamados = listaChamados.Where(x => x.Status.Equals(status.ToUpper())).ToList();

            return listaChamados;

        }

        public int Inserir(Chamado chamado)
        {

            var sql = chamado.Categorias.Aggregate(@"BEGIN TRAN
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
                       ", (current, categoria) => current + (@"
                        INSERT INTO [CHAMADOS].[dbo].[CHAMADO_CATEGORIA]
						([CODIGO_CHAMADO]
						,[CODIGO_CATEGORIA])
						VALUES
						(@CODIGO_CHAMADO
						," + categoria.Codigo + @")
                        "));
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

        public Chamado AdicionarEvento(Chamado chamado, Evento evento)
        {
            return chamado;
        }
    }
}