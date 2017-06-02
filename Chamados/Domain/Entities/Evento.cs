using System;

namespace Domain.Entities
{
    public class Evento
    {
        public string Status { get; set; }
        public DateTime Abertura { get; set; }
        public DateTime Encerrado { get; set; }
    }
}