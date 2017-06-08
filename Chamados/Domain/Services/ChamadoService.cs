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
            if (evento.Status.Equals("ENCERRADO"))
            {
                throw new Exception("O chamado não pode ser alterado pois o ultimo evento não foi encerrado");
            }
        }

        private void VerificaAtendenteCorrente(Chamado chamado, Atendente atendente)
        {
            if (chamado.Atendente.Nome!=atendente.Nome)
            {
                throw new Exception("O chamado so pode ser alterado pelo seu atendente");
            }
        }

        public Chamado AlterarAssunto(Chamado chamado, string assunto, Atendente atendente)
        {
            VerificaAtendenteCorrente(chamado, atendente);
            chamado = _iChamadoRepository.BuscarPorId(chamado.Codigo);
            VerificaUltimoEventoFinalizado(chamado);
            chamado.Assunto = assunto;
            if (chamado.Assunto.Equals(""))
            {
                throw new Exception("O Chamado precisa de um Assunto");
            }
            return _iChamadoRepository.Alterar(chamado);
        }

        public Chamado AlterarCategoria(Chamado chamado, List<Categoria> categorias, Atendente atendente)
        {
            VerificaAtendenteCorrente(chamado, atendente);
            chamado = _iChamadoRepository.BuscarPorId(chamado.Codigo);
            VerificaUltimoEventoFinalizado(chamado);
            chamado.Categorias = categorias;
            if (!categorias.Any())
            {
                throw new Exception("O Chamado precisa uma categoria");
            }
            return _iChamadoRepository.Alterar(chamado);
        }

        public Chamado AlterarFilial(Chamado chamado, Filial filial, Atendente atendente)
        {
            VerificaAtendenteCorrente(chamado, atendente);
            chamado = _iChamadoRepository.BuscarPorId(chamado.Codigo);
            VerificaUltimoEventoFinalizado(chamado);
            if (!chamado.Eventos.Any())
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
            VerificaAtendenteCorrente(chamado, atendente);
            VerificaUltimoEventoFinalizado(chamado);
            chamado.Status = "Finalizado";
            _iChamadoRepository.Alterar(chamado);
        }

        public int Inserir(Chamado chamado)
        {
            var erro = "";
            chamado.Status = "Aberto";
            if (chamado.Categorias==null)
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
            if (chamado.Eventos==null)
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