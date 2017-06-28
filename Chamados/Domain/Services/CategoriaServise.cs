using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class CategoriaServise : ICategoriaService
    {
        private readonly ICategoriaRepository _iCategoriaRepository;

        public CategoriaServise(ICategoriaRepository iCategoriaRepository)
        {
            _iCategoriaRepository = iCategoriaRepository;
        }

        public IEnumerable<Categoria> BuscarCategoria()
        {
            return _iCategoriaRepository.BuscarCategoria();
        }
    }
}