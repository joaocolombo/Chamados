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
        void Finalizar(Chamado chamado, Atendente atendente);
        Chamado BuscarPorId(int codigo);
        IEnumerable<Chamado> BuscarPorStatus(string status);
        IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente,bool finalizado);
        IEnumerable<Chamado> BuscarPorFilial(Filial filial);
        void EncaminharN2(Evento evento, Chamado chamado);
        void Encaminhar(Evento evento, Atendente atendente, Chamado chamado);

        string Teste();

    }
}