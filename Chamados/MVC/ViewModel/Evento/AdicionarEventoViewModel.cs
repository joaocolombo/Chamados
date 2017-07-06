using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Domain.Entities;

namespace MVC.ViewModel.Evento
{
    public class AdicionarEventoViewModel
    {
        public int ChamadoId { get; set; }
        public int Codigo { get; set; }
        public string NomeAtendenteNovo { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public int FilaId { get; set; }
        public int Direcao { get; set; }
        public DateTime Abertura { get; set; }
        public Atendente Atendente { get; set; }
    }
}
