using System;
using System.Reflection.Metadata;

namespace Domain.Entities
{
    public class Evento
    {

        public string Status { get; private set; }
        public int Codigo { get; private set; }
        public DateTime Abertura { get; private set; }
        public DateTime Encerramento { get; private set; }
        public Atendente Atendente { get; private set; }
        public string Descricao { get; private set; }
        public int MinutosPrevistos { get; set; }
        public int MinutosRealizados { get; set; }

        #region Validacao
        
        public Evento(int codigo, string descricao, Atendente atendente)
        {
            SetCodigo(codigo);
            SetStatus(descricao);
            SetAtendente(atendente);
        }

        public void SetCodigo(int codigo)
        {
            if (codigo != 0) throw new ArgumentException("Nao e possivel alterar codigo");
            Codigo = codigo;
        }

        public void SetStatus(string status)
        {
            if (status.Length < 3 || string.IsNullOrEmpty(status))
                throw new ArgumentException("Preencha o status com no minino 3 caracteres");
            Status = status;
        }

        public void SetAbertura(DateTime abertura)
        {
            if (abertura > Encerramento) throw new ArgumentException("Data de Abertura nao pode ser menor que de encerramento");
            Abertura = abertura;
        }
        public void SetEncerramento(DateTime encerramento)
        {
            if (encerramento>DateTime.Now) throw new ArgumentException("Data de encerramento nao pode ser maior que data atual");
            Encerramento = encerramento;
        }

        public void SetAtendente(Atendente atendente)
        {
            Atendente = atendente ?? throw new ArgumentException("O Atendente nao pode ser nulo");
        }

        public void SetDescricao(string descricao)
        {
            if(descricao.Length<3||string.IsNullOrEmpty(descricao))
                throw new ArgumentException("Preencha a descricao com no minino 3 caracteres");
            Descricao = descricao;
        }
        #endregion


    }
}