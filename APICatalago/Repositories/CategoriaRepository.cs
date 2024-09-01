using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {

        public CategoriaRepository(AppDbContext context) : base(context)
        {

        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams)
        {
            var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();
            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);
            return categoriasOrdenadas;
        }

    }
}
