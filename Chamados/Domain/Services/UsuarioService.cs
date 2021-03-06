﻿
using System;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;
using Domain.Services.Validates;

namespace Domain.Services
{
    public class UsuarioService:IUsuarioService
    {
        private readonly IUsuarioRepository _iUsuarioRepository;

        public UsuarioService(IUsuarioRepository iUsuarioRepository)
        {
            _iUsuarioRepository = iUsuarioRepository;
        }

        public Usuario Autenticar(int id, string senha)
        {
            if (id==0 || string.IsNullOrEmpty(senha))
            {
                throw new RnException("Usuario ou senha não preenchido");
            }
            return _iUsuarioRepository.Autenticar(id, senha);

        }
    }
}