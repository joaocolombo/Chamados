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
        public string Nivel => Atendente.Nivel;

        private Atendente atendente;
        public Atendente Atendente
        {
            get
            {
                if (Eventos.Any())
                {
                    var a = Eventos.OrderByDescending(x => x.Abertura).FirstOrDefault();
                    return a.Atendente;
                }
                return null;
            }
        }

    }
}