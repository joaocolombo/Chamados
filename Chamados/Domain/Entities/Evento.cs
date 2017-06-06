﻿using System;
using System.Reflection.Metadata;

namespace Domain.Entities
{
    public class Evento
    {
        public string Status { get; set; }
        public DateTime Abertura { get; set; }
        public DateTime Encerrado { get; set; }
        public Atendente Atendente { get; set; }
        public string Descricao { get; set; }
    }
}