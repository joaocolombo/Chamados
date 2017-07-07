﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain.Entities;
using Domain.Services.Interfaces.Validates;

namespace Domain.Services.Validates
{
    public class ChamadoValidate:IChamadoValidate
    {

        public string AtendenteCorrente(Chamado chamado, Atendente atendente)
        {
            return chamado.Atendente.Nome != atendente.Nome ? "O chamado so pode ser alterado pelo seu atendente" : null;
        }

        public string Finalizado(Chamado chamado)
        {
            return chamado.Finalizado ? "O chamado so pode ser alterado pois ja esta finalizado" : null;
        }

        public string PermiteAlteracao(Chamado chamado, Atendente atendente)
        {
            var erro = "";
            erro = Finalizado(chamado);
            erro += AtendenteCorrente(chamado, atendente);
            return erro;

        }

        public string NovoChamado(Chamado chamado)
        {
            var erro = "";
            if (chamado.Categorias == null)
            {
                erro = "O Chamado precisa de pelomenos uma categoria. ";
            }
            else if (!chamado.Categorias.Any())
            {
                erro = "O Chamado precisa de pelomenos uma categoria. ";
            }
            if (string.IsNullOrEmpty(chamado.Assunto))
            {
                erro += "O Chamado precisa de um assunto. ";
            }
            if (string.IsNullOrEmpty(chamado.Solicitante))
            {
                erro += "O Chamado precisa de um solicitante. ";
            }
            if (chamado.Eventos == null)
            {
                erro += "O Chamado precisa de um evendo. ";
            }
            else if (!chamado.Eventos.Any())
            {
                erro += "O Chamado precisa de um evendo. ";
            }
            else
            {
                foreach (var evento in chamado.Eventos)
                {
                    if (string.IsNullOrEmpty(evento.Descricao))
                    {
                        erro = "Necessario preencher uma descricao. ";
                    }
                    if (string.IsNullOrEmpty(evento.Status))
                    {
                        erro += "Necessario selecionar um status. ";
                    }
                }

            }

            if (chamado.Filial == null)
            {
                erro += " O Chamado precisa de uma Filial. ";
            }
            return erro;
        }

        public string PermiteAlterarAssunto(Chamado chamado, Atendente atendente, string assunto)
        {
            var erro = "";
            erro = PermiteAlteracao(chamado, atendente);
            if (string.IsNullOrEmpty(assunto))
            {
                erro = "O Chamado precisa de um Assunto. ";
            }
            return erro;
        }

        public string PermiteAlterarSolicitante(Chamado chamado, Atendente atendente, string solicitante)
        {
            var erro = "";
            erro = PermiteAlteracao(chamado, atendente);
            if (string.IsNullOrEmpty(solicitante))
            {
                erro = "O Chamado precisa de um Solicitante. ";
            }
            return erro;
        }

        public string PermiteAlterarCategoria(Chamado chamado, Atendente atendente, List<Categoria> categorias)
        {
            var erro = "";
            erro = PermiteAlteracao(chamado, atendente);
            if (categorias.Any())
            {
                erro = "O Chamado uma Categoria. ";
            }
            return erro;
        }

        public string PermiteAlterarFilial(Chamado chamado, Atendente atendente, Filial filial)
        {
            var erro = "";
            erro = PermiteAlteracao(chamado, atendente);
            if (filial==null)
            {
                erro = "O Chamado uma Categoria. ";
            }
            return erro;
        }

        public string PermiteAlterarFila(Chamado chamado, Atendente atendente, Fila fila)
        {
            var erro = "";
            erro = PermiteAlteracao(chamado, atendente);
            if (fila==null)
            {
                erro = "O Chamado uma Categoria. ";
            }
            return erro;
        }

        public string PermiteFinalizar(Chamado chamado, Atendente atendente)
        {
            return PermiteAlteracao(chamado, atendente);
        }

        public string PermiteIncluirAlterarEvento(Chamado chamado)
        {

            var erro = Finalizado(chamado);
            if (chamado.Fila != null)
            {
                erro += "Chamado esta em fila. ";
            }
            return erro;
        }

        public string PermiteAlterarAtendente(Chamado chamado)
        {
            var erro = Finalizado(chamado);
            if (chamado.Fila == null)
            {
                erro += "Chamado precisa estar em uma fila. ";
            }
            return erro;
        }
    }
}