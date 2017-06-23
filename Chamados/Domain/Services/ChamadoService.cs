using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class ChamadoService : IChamadoService
    {
        private readonly IChamadoRepository _iChamadoRepository;


        public ChamadoService(IChamadoRepository iChamadoRepository)
        {
            _iChamadoRepository = iChamadoRepository;

        }

        private void VerificaUltimoEventoFinalizado(Chamado chamado)
        {

            var evento = chamado.Eventos.OrderByDescending(x => x.Abertura).FirstOrDefault();
            if (evento.Encerrado == DateTime.MinValue)
            {
                throw new Exception("O chamado não pode ser alterado pois o ultimo evento não foi encerrado");
            }
        }

        private void VerificaAtendenteCorrente(Chamado chamado, Atendente atendente)
        {
            if (chamado.Atendente.Nome != atendente.Nome)
            {
                throw new Exception("O chamado so pode ser alterado pelo seu atendente");
            }
        }

        private void VerificaChamadoFinalizado(Chamado chamado)
        {
            if (chamado.Finalizado)
            {
                throw new Exception("O chamado so pode ser alterado pois ja esta finalizado");
            }
        }

        public Chamado AlterarAssunto(Chamado chamado, string assunto, Atendente atendente)
        {
            chamado = _iChamadoRepository.BuscarPorId(chamado.Codigo);
            VerificaAtendenteCorrente(chamado, atendente);
            VerificaUltimoEventoFinalizado(chamado);
            VerificaChamadoFinalizado(chamado);
            chamado.Assunto = assunto;
            if (chamado.Assunto.Equals(""))
            {
                throw new Exception("O Chamado precisa de um Assunto");
            }
            return _iChamadoRepository.Alterar(chamado);
        }

        public Chamado AlterarCategoria(Chamado chamado, List<Categoria> categorias, Atendente atendente)
        {
            chamado = _iChamadoRepository.BuscarPorId(chamado.Codigo);
            VerificaAtendenteCorrente(chamado, atendente);
            VerificaUltimoEventoFinalizado(chamado);
            VerificaChamadoFinalizado(chamado);
            chamado.Categorias = categorias;
            if (!categorias.Any())
            {
                throw new Exception("O Chamado precisa uma categoria");
            }
            return _iChamadoRepository.Alterar(chamado);
        }

        public Chamado AlterarFilial(Chamado chamado, Filial filial, Atendente atendente)
        {
            chamado = _iChamadoRepository.BuscarPorId(chamado.Codigo);
            VerificaAtendenteCorrente(chamado, atendente);
            VerificaChamadoFinalizado(chamado);
            if (chamado.Eventos.Any())
            {
                throw new Exception("Já Existe evento para esse chamado, não é possivel alterar");
            }
            chamado.Filial = filial;
            if (chamado.Filial == null)
            {
                throw new Exception("O Chamado Precisa de uma Filial Valida");
            }
            return _iChamadoRepository.Alterar(chamado);
        }

        public IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente, string status)
        {
            return _iChamadoRepository.BuscarPorAtendente(atendente, status);
        }

        public IEnumerable<Chamado> BuscarPorFilial(Filial filial)
        {
            return _iChamadoRepository.BuscarPorFilial(filial);
        }

        public Chamado BuscarPorId(int codigo)
        {
            return _iChamadoRepository.BuscarPorId(codigo);
        }

        public IEnumerable<Chamado> BuscarPorStatus(string status)
        {
            return _iChamadoRepository.BuscarPorStatus(status);
        }

        public void Finalizar(Chamado chamado, Atendente atendente)
        {
            var c = BuscarPorId(chamado.Codigo);
            VerificaAtendenteCorrente(c, atendente);
            VerificaUltimoEventoFinalizado(c);
            c.Status = "FINALIZADO";
            c.Finalizado = true;
            _iChamadoRepository.Alterar(c);

        }

        public int Inserir(Chamado chamado)
        {
            var erro = "";
            chamado.Status = "ABERTO";
            if (chamado.Categorias == null)
            {
                erro = "O Chamado precisa de pelomenos uma categoria.";
            }
            else if (!chamado.Categorias.Any())
            {
                erro = "O Chamado precisa de pelomenos uma categoria.";
            }
            if (String.IsNullOrEmpty(chamado.Assunto))
            {
                erro += " O Chamado precisa de um assunto.";
            }
            if (chamado.Eventos == null)
            {
                erro += " O Chamado precisa de um evendo";
            }
            else if (!chamado.Eventos.Any())
            {
                erro += " O Chamado precisa de um evendo";
            }

            if (chamado.Filial == null)
            {
                erro += " O Chamado precisa de uma Filial";
            }
            if (!erro.Equals(""))
            {
                throw new Exception(erro);
            }
            foreach (var evento in chamado.Eventos)
            {
                evento.Abertura = DateTime.Now;
            }
            return _iChamadoRepository.Inserir(chamado);
        }

        public void Encaminhar(Evento evento, Atendente atendente, Chamado chamado)
        {
            //finalizar evento anterior
            //validar
            //evento.Descricao += " Encaminhado para o Atendente " + atendente.Nome;
            //Adicionar(chamado, evento);
        }

        public void EncaminharN2(Evento evento, Chamado chamado)
        {
            ////finalizar evento anterior
            //evento.Descricao += " Encaminhado para a Fila do N2";
            //Adicionar(chamado, evento);
        }

        public string Teste()
        {
            return "sucesso";
        }
    }
}