using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace MVC.ViewModel.Fila
{
    public class FilaViewModel
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public List<Chamado> Lista { get; set; }
        public int Quantidade { get; set; }



    }
}
