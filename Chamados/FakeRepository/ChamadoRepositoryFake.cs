using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;

namespace FakeRepository
{
    public class ChamadoRepositoryFake : IChamadoRepository
    {
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
            var c= CriarChamado();
            //foreach (var evento in c.Eventos)
            //{
            //    evento.Status = "ENCERRADO";
            //}
            return c;
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
            return 10;
        }

        public Chamado AdicionarEvento(Chamado chamado, Evento evento)
        {
            return chamado;
        }
    }
}