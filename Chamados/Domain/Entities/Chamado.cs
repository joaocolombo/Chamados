using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace Domain.Entities
{
    public class Chamado
    {
        public int Codigo { get; set; }
        public string Status { get; set; }
        public List<Evento> Eventos { get; set; }
        public Filial Filial { get; set; }
        public string Assunto { get; set; }
        public List<Categoria> Categorias { get; set; }
        private bool finalizado;

        public bool Finalizado
        {
            get { return finalizado; }
            set
            {
                if (Finalizado)
                {
                    throw new Exception("Chamado ja finalizado nao pode ser reaberto");
                }
                finalizado = value;
            }
        }

        public string Nivel => Atendente.Nivel;

        public Atendente Atendente
        {
            get
            {
                if (!Eventos.Any()) return null;
                var a = Eventos.OrderByDescending(x => x.Abertura).FirstOrDefault();
                return a.Atendente;
            }
        }


    }
}