using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;
using Domain.Services.Interfaces.Validates;

namespace Domain.Services
{
    public class ChamadoService : IChamadoService
    {
        private readonly IChamadoRepository _iChamadoRepository;
        private readonly IChamadoValidate _iChamadoValidate;
        private readonly IEventoValidate _iEventoValidate;
        private readonly IFilialService _iFilialService;


        public ChamadoService(IEventoValidate iEventoValidate, IChamadoRepository iChamadoRepository, IFilialService iFilialService, IChamadoValidate iChamadoValidate)
        {
            _iChamadoRepository = iChamadoRepository;
            _iFilialService = iFilialService;
            _iChamadoValidate = iChamadoValidate;
            _iEventoValidate = iEventoValidate;
        }

        public Chamado AlterarAssunto(int codigo, string assunto, Atendente atendente)
        {
            var chamado = _iChamadoRepository.BuscarPorId(codigo);
            var erro = _iChamadoValidate.PermiteAlterarAssunto(chamado, atendente, assunto);
            if (!string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }
            chamado.Assunto = assunto;
            return _iChamadoRepository.Alterar(chamado);

        }
        public Chamado AlterarSolicitante(int codigo, string solicitante, Atendente atendente)
        {
            var chamado = _iChamadoRepository.BuscarPorId(codigo);
            var erro = _iChamadoValidate.PermiteAlterarSolicitante(chamado, atendente, solicitante);
            if (!string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }
            chamado.Solicitante = solicitante;
            return _iChamadoRepository.Alterar(chamado);
        }

        public Chamado AlterarCategoria(int codigo, List<Categoria> categorias, Atendente atendente)
        {
            var chamado = _iChamadoRepository.BuscarPorId(codigo);
            var erro = _iChamadoValidate.PermiteAlterarCategoria(chamado, atendente, categorias);
            if (!string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }
            chamado.Categorias = categorias;
            return _iChamadoRepository.Alterar(chamado);
        }

        public Chamado AlterarFilial(int codigo, Filial filial, Atendente atendente)
        {
            var chamado = _iChamadoRepository.BuscarPorId(codigo);
            filial = _iFilialService.BuscarPorCodigo(filial.Codigo);
            var erro = _iChamadoValidate.PermiteAlterarFilial(chamado, atendente, filial);

            if (!string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }
            chamado.Filial = filial;
            return _iChamadoRepository.Alterar(chamado);
        }

        public void AlterarFila(int codigo, Fila fila, Atendente atendente)
        {
            var chamado = _iChamadoRepository.BuscarPorId(codigo);
            var erro = _iChamadoValidate.PermiteAlterarFila(chamado, atendente, fila);
            if (!string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }

            _iChamadoRepository.AdicionarNaFila(codigo, fila);
        }
        //Este metodo vem somente do Evento.Adicionar e Evento.Assumir
        public void RemoverFila(int codigo)
        {
            _iChamadoRepository.RemoveDaFila(codigo);
        }


        public void Finalizar(int codigo, Atendente atendente)
        {
            var c = BuscarPorId(codigo);
            var erro = _iChamadoValidate.PermiteFinalizar(c, atendente);
            if (!string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }
            c.Status = "FINALIZADO";
            c.Finalizado = true;
            _iChamadoRepository.Alterar(c);
        }

        public int Inserir(Chamado chamado)
        {
            var erro = _iChamadoValidate.NovoChamado(chamado);
            foreach (var evento in chamado.Eventos)
            {
                erro += _iEventoValidate.NovoEvento(evento, chamado.Atendente, chamado);
            }
            if (!erro.Equals(""))
            {
                throw new Exception(erro);
            }

            chamado.Status = "ABERTO";
            foreach (var evento in chamado.Eventos)
            {
                evento.Abertura = DateTime.Now;
            }
            try
            {
                return _iChamadoRepository.Inserir(chamado);

            }
            catch (Exception e)
            {
                if (chamado.Imagens != null)
                {
                    foreach (var imagem in chamado.Imagens)
                    {
                        System.IO.File.Delete(@"C:\temp\" + imagem);
                    }

                }
                return 0;
            }
        }

        public Chamado BuscarPorIdEvento(int codigoEvento)
        {
            return _iChamadoRepository.BuscarPorIdEvento(codigoEvento);
        }

        public IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente, bool finalizado)
        {
            return _iChamadoRepository.BuscarPorAtendente(atendente, finalizado);
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

        public IEnumerable<object> SelectGenerico(string tabela, string parametros, string draw, string orderby, string orderbyDirecao)
        {
            return _iChamadoRepository.SelectGenerico(tabela, parametros, draw, orderby, orderbyDirecao);
        }

        public int TotalRegistros(string tabela, string parametros)
        {
            return 100;
        }

        public Chamado AdicionarImagem(int codigo, string nomeArquivo, Atendente atendente)
        {
            var chamado = _iChamadoRepository.BuscarPorId(codigo);
            var erro = _iChamadoValidate.AtendenteCorrente(chamado, atendente);
            if (!string.IsNullOrEmpty(erro))
            {
                throw new Exception(erro);
            }
            _iChamadoRepository.AdicionarImagem(codigo, nomeArquivo);
            return _iChamadoRepository.BuscarPorId(codigo);

        }
    }
}