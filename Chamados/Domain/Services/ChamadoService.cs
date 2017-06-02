﻿using System;
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

        public void Finalizar(Chamado chamado)
        {
            chamado.Status = "Finalizado";
            _iChamadoRepository.Alterar(chamado);
        }

        public int Inserir(Chamado chamado)
        {
            //validar
            return _iChamadoRepository.Inserir(chamado);
        }

        public string Teste()
        {
            return "sucesso";
        }
    }
}