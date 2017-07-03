using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.ViewModel
{
    public class AdicionarEventoViewModel
    {
        public string ChamadoId { get; set; }
        public string NomeAtendenteAtual { get; set; }
        public string NomeAtendenteNovo { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
    }
}
