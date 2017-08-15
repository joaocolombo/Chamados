using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace MVC.ViewModel.Home
{
    public class InserirChamadoViewModel
    {
        public string Assunto { get; set; }
        public string CodigoFilial { get; set; }
        public string Solicitante { get; set; }
        public IEnumerable<int> Categorias { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public string NomeAtendente { get; set; }
        public bool Geral { get; set; }
        public int MinutosRealizados { get; set; }
        public int MinutosPrevistos { get; set; }

    }
}
