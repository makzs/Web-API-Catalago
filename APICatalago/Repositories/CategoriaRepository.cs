using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APICatalago.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {

        public CategoriaRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParams)
        {
            var categorias = await GetAllAsync();

            var  categoriasOrdenadas = categorias.OrderBy(c=>c.CategoriaId).AsQueryable();

            var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParams.PageNumber, categoriasParams.PageSize);
            return resultado;
        }

        public async Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriaFiltroNome categoriaFiltroParams)
        {
            var categorias = await GetAllAsync();

            if (!string.IsNullOrEmpty(categoriaFiltroParams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriaFiltroParams.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriaFiltroParams.PageNumber, categoriaFiltroParams.PageSize);

            return categoriasFiltradas;
        }
    }
}
