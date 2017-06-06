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
        private Atendente atendente;
        public string Nivel => Atendente.Nivel;

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