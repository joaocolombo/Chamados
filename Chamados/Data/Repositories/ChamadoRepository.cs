using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class ChamadoRepository : IChamadoRepository
    {
        public Chamado Alterar(Chamado chamado)
        {
            throw new NotImplementedException();
        }

        public Chamado BuscarPorId(int codigo)
        {
            var a1 = new Atendente() {Nome = "Joao", Nivel = "N2"};
            var a2 = new Atendente() {Nome = "Douglas", Nivel = "N1"};
            var e1 = new Evento()
            {
                Abertura = DateTime.Now.AddDays(-2),
                Atendente = a2,
                Descricao = "evento1",
                Encerrado = DateTime.Now.AddDays(-1),
                Status = "Encerrado"
            };
            var e2 = new Evento()
            {
                Abertura = DateTime.Now.AddDays(-1),
                Atendente = a2,
                Descricao = "Encaminhado N2",
                Encerrado = DateTime.Now,
                Status = "Encerrado"
            };
            var e3 = new Evento()
            {
                Abertura = DateTime.Now,
                Atendente = a1,
                Descricao = "evento 3",
                Status = "Aberto"
            };
            List<Evento> lista = new List<Evento>();
            lista.Add(e1);
            lista.Add(e2);
            lista.Add(e3);
            return new Chamado() {Codigo = codigo, Eventos = lista};
        }

        public int Inserir(Chamado chamado)
        {
            throw new NotImplementedException();
        }
    }
}