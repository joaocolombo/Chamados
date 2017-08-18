using System;
using System.Reflection.Metadata;

namespace Domain.Entities
{
    public class Evento
    {
        private int codigo;
        private string status;
        private DateTime abertura;
        private DateTime encerramento;
        private Atendente atendente;
        private string descricao;
        public int MinutosPrevistos { get; set; }
        public int MinutosRealizados { get; set; }

        #region Validacao/Sets

        public Evento(int codigo, string descricao, Atendente atendente)
        {
            Codigo = codigo;
            Descricao = descricao;
            Atendente = atendente;
        }

        private Evento()
        {

        }

        public int Codigo
        {
            get { return codigo; }
            set
            {
                if (codigo != 0) throw new ArgumentException("Nao e possivel alterar codigo");
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
        public DateTime Abertura
        {
            get { return abertura; }
            set
            {
                if (encerramento != DateTime.MinValue)
                    if (value > encerramento) throw new ArgumentException("Data de Abertura nao pode ser menor que de encerramento");

                abertura = value;
            }
        }
        public DateTime Encerramento
        {
            get { return encerramento; }
            set
            {
                if (value > DateTime.Now) throw new ArgumentException("Data de encerramento nao pode ser maior que data atual");
                encerramento = value;
            }
        }

        public Atendente Atendente
        {
            get { return atendente; }
            set
            {
                atendente = value ?? throw new ArgumentException("O Atendente nao pode ser nulo");
            }
        }
        public string Descricao
        {
            get { return descricao; }
            set
            {
                if (value.Length < 3 || string.IsNullOrEmpty(value))
                    throw new ArgumentException("Preencha a descricao com no minino 3 caracteres");
                descricao = value;
            }
        }

        #endregion


    }
}