using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace Data.Repositories
{
    public static class CategoriaRepository
    {
        public static string InserirSql(Chamado chamado)
        {
            return chamado.Categorias.Aggregate("", (current, categoria) => current + (@"
                        INSERT INTO [CHAMADOS].[dbo].[CHAMADO_CATEGORIA]
						([CODIGO_CHAMADO]
						,[CODIGO_CATEGORIA])
						VALUES
						(@CODIGO_CHAMADO
						," + categoria.Codigo + @")
                        "));
        }

        public static List<Categoria> BuscarCategoriasPorChamado(int codigoChamado)
        {
            
        }
    }
}