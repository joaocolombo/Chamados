﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IChamadoRepository
    {
        Chamado Alterar(Chamado chamado);
        int Inserir(Chamado chamado);
        Chamado BuscarPorId(int codigo);
        IEnumerable<Chamado> BuscarPorStatus(string status);
        IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente, bool finalizado);
        IEnumerable<Chamado> BuscarPorFilial(Filial filial);
        IEnumerable<Chamado> BuscarPorFila(Fila fila);
        Chamado BuscarPorIdEvento(int codigoEvento);
        IEnumerable<object> SelectGenerico(string tabela, string parametros, string draw, string orderby, string orderbyDirecao, string start, string length);
        int TotalRegistros(string tabela, string parametros);
        Chamado AdicionarNaFila(int codigo, Fila fila );
        bool ChamadoEmFila(int codigoChamado);
        Chamado RemoveDaFila(int codigo);
        void AdicionarImagem(int codigo, string nomeArquivo);
        List<string> BuscarImagensPorChamado(int codigo);
        IEnumerable<Chamado> BuscarTodos();
    }
}