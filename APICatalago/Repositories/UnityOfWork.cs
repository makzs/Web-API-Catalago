using APICatalago.Context;

namespace APICatalago.Repositories
{
    public class UnityOfWork : IUnitOfWork
    {
        private IProdutoRepository _ProdutoRepo;

        private ICategoriaRepository _CategoriaRepo;

        public AppDbContext _context;

        public UnityOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _ProdutoRepo = _ProdutoRepo ?? new ProdutoRepository(_context);
            }
        }
        
        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _CategoriaRepo = _CategoriaRepo ?? new CategoriaRepository(_context);
            }
        }

        public async Task commitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
