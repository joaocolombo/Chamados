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

        
        private int codigo;
        private string status;
        private List<Evento> eventos;
        private Filial filial;
        private string assunto;
        private string solicitante;
        private List<Categoria> categorias;
        private bool finalizado;


        public List<string> Imagens { get; set; }
        public string Nivel => Atendente?.Nivel;

        public int MinutosPrevistos
        {
            get
            { return eventos.Any() ? eventos.Sum(x => x.MinutosPrevistos) : 0; }
        }
        public int MinutosRealizados
        {
            get
            { return eventos.Any() ? eventos.Sum(x => x.MinutosRealizados) : 0; }
        }
        public Atendente Atendente
        {
            get
            {
                if (!eventos.Any()) return null;
                var a = eventos.OrderByDescending(x => x.Abertura).FirstOrDefault();
                return a.Atendente;
            }
        }


        #region Validacao/Sets
        
        public Chamado(int codigo, string status, string assunto, string solicitante)
        {
            Codigo = codigo;
            Status=status;
            Assunto =assunto;
            Solicitante=solicitante;
        }

        private Chamado()
        {
            
        }
        public bool Finalizado
        {
            get { return finalizado; }
            set
            {
                if (finalizado)
                    throw new ArgumentException("Chamado ja finalizado nao pode ser reaberto");
                finalizado = value;
            }
        }

        public int Codigo
        {
            get { return codigo; }
            set
            {
                if (codigo != 0) throw new AggregateException("O codigo do Chamado nunca pode ser alterado");
                codigo = value;
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                if (value.Length < 3 || string.IsNullOrEmpty(value))
                    throw new ArgumentException("Preencha o status com no minino 3 caracteres");
                status = value;
            }
        }
        public string Assunto
        {
            get { return assunto; }
            set
            {
                if (value.Length < 3 || string.IsNullOrEmpty(value))
                    throw new ArgumentException("Preencha o assunto com no minino 3 caracteres");
                assunto = value;
            }
        }
        public string Solicitante
        {
            get { return solicitante; }
            set
            {
                if (value.Length < 3 || string.IsNullOrEmpty(value))
                    throw new ArgumentException("Preencha o solicitante com no minino 3 caracteres");
                solicitante = value;
            }
        }

        public List<Evento> Eventos
        {
            get { return eventos; }
            set
            {
                if (!value.Any())
                    throw new ArgumentException("Lista de Eventos vazia");
                eventos = value;
            }
        }
        
        public Filial Filial
        {
            get { return filial; }
            set
            {
                if (value == null || string.IsNullOrEmpty(value.Codigo))
                    throw new ArgumentException("Filial Invalida");
                filial = value;
            }
        }

        public List<Categoria> Categorias
        {
            get { return categorias; }
            set
            {
                if (!value.Any())
                    throw new ArgumentException("Lista de Categorias esta vazia");

                if (value.Any(categoria => categoria.Codigo == 0))
                    throw new ArgumentException("Exite uma categoria invalida");
                categorias = value;
            }
        }


        #endregion

    }
}