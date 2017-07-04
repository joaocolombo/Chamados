
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Fila
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public List<Chamado> Lista { get; set; }
    }
}
