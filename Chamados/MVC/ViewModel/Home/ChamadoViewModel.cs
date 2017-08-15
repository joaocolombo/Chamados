using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using MVC.ViewModel.Evento;

namespace MVC.ViewModel.Home
{
    public class ChamadoViewModel
    {
        public int Codigo { get; set; }
        public Domain.Entities.Fila Fila { get; set; }
        public string Status { get; set; }
        public List<AdicionarEventoViewModel> Eventos { get; set; }
        public Filial Filial { get; set; }
        public string Assunto { get; set; }
        public string Solicitante { get; set; }
        public List<Categoria> Categorias { get; set; }

        public int MinutosPrevistos
        {
            get { return Eventos.Any() ? Eventos.Sum(x => x.MinutosPrevistos) : 0; }
        }

        public int MinutosRealizados
        {
            get { return Eventos.Any() ? Eventos.Sum(x => x.MinutosRealizados) : 0; }
        }

        private bool _finalizado;

        public bool Finalizado
        {
            get { return _finalizado; }
            set
            {
                if (Finalizado)
                    throw new Exception("Chamado ja finalizado nao pode ser reaberto");
                _finalizado = value;
            }
        }

        private string _nivel;

        public string Nivel
        {
            get { return Atendente?.Nivel; }
            set { _nivel = value; }
        }

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