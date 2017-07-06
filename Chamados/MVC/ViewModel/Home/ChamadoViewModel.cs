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
            private bool finalizado;

            public bool Finalizado
            {
                get { return finalizado; }
                set
                {
                    if (Finalizado)
                    {
                        throw new Exception("Chamado ja finalizado nao pode ser reaberto");
                    }
                    finalizado = value;
                }
            }

            public string Nivel => Atendente.Nivel;

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