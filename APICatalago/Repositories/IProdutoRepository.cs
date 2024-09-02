using APICatalago.Models;
using APICatalago.Pagination;

namespace APICatalago.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
        Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
        Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
    }
}
