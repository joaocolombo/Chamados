using System;
using System.Collections.Generic;

using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.Services.Interfaces;
using FakeRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainTest.Domain
{
    [TestClass]
    public class ChamadoServiceTest
    {
        //int Inserir(Chamado chamado);
        //Chamado AlterarFilial(Chamado chamdo, Filial filial, Atendente atendente);
        //Chamado AlterarAssunto(Chamado chamado, string assunto, Atendente atendente);
        //Chamado AlterarCategoria(Chamado chamado, List<Categoria> categorias, Atendente atendente);
        //void Finalizar(Chamado chamado, Atendente atendente);

        private Chamado criarChamadoValido()
        {
            List<Categoria> listaCategoria = new List<Categoria>();
            listaCategoria.Add(new Categoria() { Codigo = 1, Descricao = "teste" });
            List<Evento> listaEventos = new List<Evento>();
            listaEventos.Add(new Evento() { Atendente = new Atendente() { Nome = "teste", Nivel = "2" }, Descricao = "teste" });
            var chamado = new Chamado()
            {
                Codigo = 1,
                Assunto = "assuntoteste",
                Categorias = listaCategoria,
                Eventos = listaEventos,
                Filial = new Filial()
            };
            return chamado;
        }

        [TestMethod]
        public void ChamadoInserirTrue()
        {

            var chamado = criarChamadoValido();
            var chamadoService = new ChamadoService(new ChamadoRepositoryFake());
         
            var resultado = chamadoService.Inserir(chamado);
        
            Assert.AreEqual(resultado,10);

        }
        [TestMethod]
        public void AlterarFilialTrue()
        {
            var chamado = criarChamadoValido();
            var chamadoService = new ChamadoService(new ChamadoRepositoryFake());

            var resultado = chamadoService.AlterarFilial
                (chamado, new Filial(), new Atendente() { Nome = "teste", Nivel = "2" });

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AlterarAssuntoTrue()
        {
            var chamado = criarChamadoValido();
            var chamadoService = new ChamadoService(new ChamadoRepositoryFake());


            var resultado = chamadoService.AlterarAssunto
                (chamado, "alterção de assunto", new Atendente() { Nome = "teste", Nivel = "2" });

            Assert.IsNotNull(resultado);
        }


        [TestMethod]
        public void AlterarCategoriaTrue()
        {
            var chamado = criarChamadoValido();
            var chamadoService = new ChamadoService(new ChamadoRepositoryFake());
            List<Categoria> l = new List<Categoria>();
            l.Add(new Categoria(){Codigo = 1,Descricao = "teste",Grupo = "1"});
            l.Add(new Categoria(){Codigo = 2,Descricao = "teste2",Grupo = "2"});
            l.Add(new Categoria(){Codigo = 20,Descricao = "teste3",Grupo = "2"});


            var resultado = chamadoService.AlterarCategoria
                (chamado, l, new Atendente() { Nome = "teste", Nivel = "2" });

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void FinalizarFail()
        {
            var chamado = criarChamadoValido();
            var chamadoService = new ChamadoService(new ChamadoRepositoryFake());

            chamadoService.Finalizar(chamado, new Atendente() {Nome = "teste", Nivel = "2"});

            Assert.IsTrue(true);
        }
    }
}
