using System.Collections.Generic;

namespace Domain.Entities
{
    public class Chamado
    {
        public string Status { get; set; }
        public List<Evento> Eventos { get; set; }
    }
}