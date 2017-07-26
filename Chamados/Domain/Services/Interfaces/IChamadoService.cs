using System.Collections;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface IChamadoService
    {
        int Inserir(Chamado chamado);
        Chamado AlterarFilial(int codigo, Filial filial, Atendente atendente);
        Chamado AlterarAssunto(int codigo, string assunto, Atendente atendente);
        Chamado AlterarCategoria(int codigo, List<Categoria> categorias, Atendente atendente);
        Chamado AlterarSolicitante(int codigo, string solicitante , Atendente atendente);
        Chamado BuscarPorIdEvento(int codigoEvento);
        Chamado AdicionarImagem(int codigo, string nomeArquivo, Atendente atendente);
        void Finalizar(int codigo, Atendente atendente);
        Chamado BuscarPorId(int codigo);
        IEnumerable<Chamado> BuscarPorStatus(string status);
        IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente,bool finalizado);
        IEnumerable<Chamado> BuscarPorFilial(Filial filial);
        void AlterarFila(int codigo, Fila fila, Atendente atendente);
        void RemoverFila(int codigo);
        IEnumerable<object> SelectGenerico(string tabela, string parametros, string draw, string orderby, string orderbyDirecao);
        int TotalRegistros(string tabela, string parametros);

    }
}