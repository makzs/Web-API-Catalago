using APICatalago.Context;
using APICatalago.Models;

namespace APICatalago.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(AppDbContext context) : base(context)
        {

        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c=> c.CategoriaId == id);
        }
    }
}
