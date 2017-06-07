using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class ChamadoService : IChamadoService
    {
        private readonly IChamadoRepository _iChamadoRepository;

        public ChamadoService(IChamadoRepository iChamadoRepository)
        {
            _iChamadoRepository = iChamadoRepository;   
        }

        public Chamado Alterar(Chamado chamado)
        {
            //validar
            return _iChamadoRepository.Alterar(chamado);
        }

        public IEnumerable<Chamado> BuscarPorAtendente(Atendente atendente, string status)
        {
            return _iChamadoRepository.BuscarPorAtendente(atendente, status);
        }

        public IEnumerable<Chamado> BuscarPorFilial(Filial filial)
        {
            return _iChamadoRepository.BuscarPorFilial(filial);
        }

        public Chamado BuscarPorId(int codigo)
        {
            return _iChamadoRepository.BuscarPorId(codigo);
        }

        public IEnumerable<Chamado> BuscarPorStatus(string status)
        {
            return _iChamadoRepository.BuscarPorStatus(status);
        }

        public void Finalizar(Chamado chamado)
        {
            chamado.Status = "Finalizado";
            _iChamadoRepository.Alterar(chamado);
        }

        public int Inserir(Chamado chamado)
        {
            //validar
            chamado.Status="Aberto";
            return _iChamadoRepository.Inserir(chamado);
        }

        public string Teste()
        {
            return "sucesso";
        }
    }
}