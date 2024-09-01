using APICatalago.Models;
using APICatalago.Pagination;

namespace APICatalago.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams);
        PagedList<Categoria> GetCategoriasFiltroNome(CategoriaFiltroNome categoriaFiltroParams);

    }
}
