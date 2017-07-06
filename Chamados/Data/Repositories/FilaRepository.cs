using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.Entities;
using Domain.Repositories;

namespace Data.Repositories
{
    public class FilaRepository : IFilaRepository
    {
        private readonly IChamadoRepository _iChamadoRepository;

        public FilaRepository(IChamadoRepository iChamadoRepository)
        {
            _iChamadoRepository = iChamadoRepository;
        }

        public IEnumerable<Fila> BuscarFila()
        {
            var sql = @"SELECT A.CODIGO
	                  ,A.DESCRICAO
	                  FROM FILA AS A";
           var fila = ChamadosDb.Conecection().Query<Fila>(sql);
        }

        public Fila BuscarPorId(int codigo)
        {
            var sql = @"SELECT A.CODIGO
	                  ,A.DESCRICAO
	                  FROM FILA AS A
                      WHERE CODIGO = @CODIGO";
            return ChamadosDb.Conecection().Query<Fila>(sql, new {CODIGO=codigo}).FirstOrDefault();
        }
    }
}