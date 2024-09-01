using APICatalago.Models;
using APICatalago.Pagination;

namespace APICatalago.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
