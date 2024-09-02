using APICatalago.Models;
using APICatalago.Pagination;

namespace APICatalago.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParams);
        Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriaFiltroNome categoriaFiltroParams);

    }
}
