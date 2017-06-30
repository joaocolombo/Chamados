using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace MVC.ViewModel
{
    public class AlterarCategoriaViewModel
    {
        public IEnumerable<int> Categorias { get; set; }
        public string Id { get; set; }
        public string Atendente { get; set; }
    }
}
