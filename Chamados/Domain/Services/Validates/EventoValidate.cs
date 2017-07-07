﻿using System;
using System.Diagnostics.Tracing;
using System.Linq;
using Domain.Entities;
using Domain.Services.Interfaces;
using Domain.Services.Interfaces.Validates;

namespace Domain.Services.Validates
{
    public class EventoValidate:IEventoValidate
    {
        private string AtendenteCorrente(Atendente atendenteAtual, Atendente atendenteNovo)
        {
            if (atendenteAtual.Nome != atendenteNovo.Nome)
            {
                return "O Evento não pode ser adicionado/alterado por esse atendente. ";
            }
            return null;
        }

        private string Finalizado(Evento evento)
        {
            if (evento.Encerramento != new DateTime(1900, 01, 01))
            {
                return "Evento ja foi finalizado. ";
            }
            return null;
        }

         public string NovoEvento(Evento evento, Atendente atendente, Chamado chamado)
        {
            var erro = "";
            erro += AtendenteCorrente(chamado.Atendente, atendente);

            if (string.IsNullOrEmpty(evento.Descricao))
            {
                erro = "Necessario preencher uma descricao. ";
            }
            if (string.IsNullOrEmpty(evento.Status))
            {
                erro += "Necessario selecionar um status. ";
            }
            return erro;
        }


        public string PermiteAlterarStatus(Evento evento, Atendente atendente, string status)
        {
            var erro = AtendenteCorrente(atendente, evento.Atendente);
            erro += Finalizado(evento);
            if (string.IsNullOrEmpty(status))
            {
                erro += "O status precisa ser preenchido. ";
            }
            return erro;

        }

        public string PermiteAlterarDescricao(Evento evento, Atendente atendente, string descricao)
        {
            var erro = AtendenteCorrente(atendente, evento.Atendente);
            erro += Finalizado(evento);
            if (string.IsNullOrEmpty(descricao))
            {
                erro += "O status precisa ser preenchido. ";
            }
            if (descricao.Length<5)
            {
                erro += "O status precisa ter mais que 5 caracteres. ";
            }
            return erro;

        }
    }
}