using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace Domain.Entities
{
    public class Chamado
    {


        public int Codigo { get; private set; }
        public string Status { get; private set; }
        public List<Evento> Eventos { get; private set; }
        public Filial Filial { get; private set; }
        public string Assunto { get; private set; }
        public string Solicitante { get; private set; }
        public List<Categoria> Categorias { get; private set; }
        public List<string> Imagens { get; set; }
        public bool Finalizado { get; private set; }

        public string Nivel
        {
            get
            {
               return  Eventos.FirstOrDefault(x =>x.Encerramento== Eventos.Max(y=>y.Encerramento)).Atendente.Nivel;
            }
        }

        public int MinutosPrevistos
        {
            get
            { return Eventos.Any() ? Eventos.Sum(x => x.MinutosPrevistos) : 0; }
        }
        public int MinutosRealizados
        {
            get
            { return Eventos.Any() ? Eventos.Sum(x => x.MinutosRealizados) : 0; }
        }


        public Atendente Atendente
        {
            get
            {
                if (!Eventos.Any()) return null;
                var a = Eventos.OrderByDescending(x => x.Abertura).FirstOrDefault();
                return a.Atendente;
            }
        }


        #region Validacao

        public Chamado(int codigo, string status, string assunto, string solicitante)
        {
            Codigo = codigo;
            SetStatus(status);
            SetAssunto(assunto);
            SetSolicitante(solicitante);
        }

        public void SetFinalziado(bool finalizado)
        {
            {
                if (Finalizado)
                    throw new ArgumentException("Chamado ja finalizado nao pode ser reaberto");
                Finalizado = finalizado;
            }
        }
        public void SetCodigo(int coodigo)
        {
            if (Codigo != 0) throw new AggregateException("O codigo do Chamado nunca pode ser alterado");
            Codigo = Codigo;
        }


        public void SetStatus(string status)
        {
            if (status.Length < 3 || string.IsNullOrEmpty(status))
                throw new ArgumentException("Preencha o status com no minino 3 caracteres");
            Status = status;
        }

        public void SetAssunto(string assunto)
        {
            if (assunto.Length < 3 || string.IsNullOrEmpty(assunto))
                throw new ArgumentException("Preencha o assunto com no minino 3 caracteres");
            Assunto = assunto;
        }

        public void SetSolicitante(string solicitante)
        {
            if (solicitante.Length < 3 || string.IsNullOrEmpty(solicitante))
                throw new ArgumentException("Preencha o solicitante com no minino 3 caracteres");
            Solicitante = solicitante;
        }

        public void SetEventos(List<Evento> eventos)
        {
            if (!eventos.Any())
                throw new ArgumentException("Lista de Eventos vazia");
            Eventos = eventos;
        }

        public void SetFilial(Filial filial)
        {
            if (filial == null || string.IsNullOrEmpty(filial.Codigo))
                throw new ArgumentException("Filial Invalida");
            Filial = filial;

        }
        public void SetCategoria(List<Categoria> categorias)
        {
            if (!categorias.Any())
                throw new ArgumentException("Lista de Categorias esta vazia");

            if (categorias.Any(categoria => categoria.Codigo != 0))
                throw new ArgumentException("Exite uma categoria invalida");
            Categorias = categorias;
        }

        #endregion

    }
}